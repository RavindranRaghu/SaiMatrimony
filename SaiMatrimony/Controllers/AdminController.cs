﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SaiMatrimony.Auth;
using SaiMatrimony.Common;
using SaiMatrimony.Dto;
using SaiMatrimony.Models;

namespace SaiMatrimony.Controllers
{
    [AdminUser("basic")]
    public class AdminController : Controller
    {
        SaiMatrimonyDb db;

        public AdminController(SaiMatrimonyDb dbContext)
        {
            db = dbContext;
        }

        
        public IActionResult Index()
        {
            return View();
        }

        #region UserManagement

        public IActionResult Userwithrole(string firstName, string lastName, string email, string all)
        {
            CommonFunction common = new CommonFunction();
            List<LoginUserProfile> userDetails = new List<LoginUserProfile>();
            List<UserBasic> users = db.UserBasic.ToList();
            List<UserRoleMap> roleMaps = db.UserRoleMap.ToList();

            userDetails = (from user in users
                           where ! roleMaps.Any(x=>x.UserIdSystem == user.UserIdSystem)
                           select new LoginUserProfile
                           {
                               UserId = user.UserIdId,
                               FirstName = user.FirstName,
                               MiddleName = user.MiddleName,
                               LastName = user.LastName,
                               Email = user.Email,
                               Role = "No Role"
                           }).ToList();

            if (all == "y")
            {
                userDetails = userDetails.Union(from user in db.UserBasic
                                                join map in db.UserRoleMap on user.UserIdSystem equals map.UserIdSystem
                                                select new LoginUserProfile
                                                {
                                                    UserId = user.UserIdId,
                                                    FirstName = user.FirstName,
                                                    MiddleName = user.MiddleName,
                                                    LastName = user.LastName,
                                                    Email = user.Email,
                                                    Role = map.IsAdmin ? "Admin" : "Basic"
                                                }).ToList();

            }

            if (!common.CheckIsNullOrEmpty(firstName))
            {
                userDetails = userDetails.Where(x => x.FirstName.ToLower().Contains(firstName)).ToList();
            }

            if (!common.CheckIsNullOrEmpty(lastName))
            {
                userDetails = userDetails.Where(x => x.LastName.ToLower().Contains(lastName)).ToList();
            }

            if (!common.CheckIsNullOrEmpty(email))
            {
                userDetails = userDetails.Where(x => x.Email.ToLower().Contains(email)).ToList();
            }

            return Json(userDetails);
        }

        public IActionResult SearchRole(string search)
        {
            List<string> roles = new List<string>();
            roles.Add("user");
            roles.Add("admin");
            return Json(roles);
        }

        public IActionResult ManageUserRole ([FromBody] UserRoleMap userRoleDto)
        {
            string key = "n";
            string value = "Error Updating the User Role";
            string userId = AuthSession.GetUserId(HttpContext, "userId");
            UserRoleMap userRoleMap = new UserRoleMap();
            try
            {
                if (userRoleDto != null)
                {
                    bool isUser = Int32.TryParse(userRoleDto.UserIdSystem, out int userid);
                    if (isUser)
                    {
                        UserBasic userBasic = db.UserBasic.Where(x => x.UserIdId == userid).FirstOrDefault();
                        userRoleMap = db.UserRoleMap.Where(x => x.UserIdSystem == userBasic.UserIdSystem).FirstOrDefault();
                        if (userRoleMap != null)
                        {
                            db.UserRoleMap.Remove(userRoleMap);
                        }
                        if (userRoleDto.MapId == 1)
                        {
                            userRoleMap = new UserRoleMap
                            {
                                UserIdSystem = userBasic.UserIdSystem,
                                IsAdmin = userRoleDto.IsAdmin,
                                UpdateByName = userBasic.FirstName + " " + userBasic.MiddleName + " " + userBasic.LastName,
                                UpdateById = userId,
                                UpdatedDate = DateTime.UtcNow
                            };
                            db.UserRoleMap.Attach(userRoleMap);
                            db.UserRoleMap.Add(userRoleMap);
                        }
                        db.SaveChanges();
                    }
                }
            }            
            catch (Exception)
            {
                return Json(new KeyValuePair<string, string>(key, value));
            }
            return Json(new KeyValuePair<string, string>("y", "Role Updated Successfully"));
        }

        #endregion

        #region ReviewProfile
        public IActionResult Profile(string firstName, string lastName, string email, string all)
        {
            CommonFunction common = new CommonFunction();
            List<ProfileDetails> profileDetails = new List<ProfileDetails>();
            List<ProfileDetails> profiles = db.ProfileDetails.ToList();

            if (all == "n")
            {
                profiles = profiles.Where(x => !x.IsProfileApproved).ToList();

            }

            profileDetails = (from user in profiles
                              select new ProfileDetails
                              {
                                   ProfileId = user.ProfileId,
                                   FirstName = user.FirstName,
                                   MiddleName = user.MiddleName,
                                   LastName = user.LastName,
                                   Email = user.Email,
                                   Profession = user.Profession,
                                   Education = user.Education
                               }).ToList();

            if (!common.CheckIsNullOrEmpty(firstName))
            {
                profileDetails = profileDetails.Where(x => x.FirstName.ToLower().Contains(firstName)).ToList();
            }

            if (!common.CheckIsNullOrEmpty(lastName))
            {
                profileDetails = profileDetails.Where(x => x.LastName.ToLower().Contains(lastName)).ToList();
            }

            if (!common.CheckIsNullOrEmpty(email))
            {
                profileDetails = profileDetails.Where(x => x.Email.ToLower().Contains(email)).ToList();
            }

            return Json(profileDetails);
        }

        public IActionResult ProcessProfile(int profileid, string approved)
        {
            ProfileDetails profile = new ProfileDetails();
            var hasprofile = db.ProfileDetails.Where(x => x.ProfileId == profileid);
            if (hasprofile.Any())
            {
                profile = hasprofile.FirstOrDefault();                
                profile.IsProfileApproved = (approved == "y");
                db.SaveChanges();
                return Json("y");
            }
            else
            {
                return Json("n");
            }
        }

        #endregion



    }
}