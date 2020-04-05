using System;
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

        public IActionResult Userwithrole(string firstName, string lastName, string email, string all)
        {
            StringFunctions common = new StringFunctions();
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
                                UpdateById = userBasic.UserIdSystem,
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

    }
}