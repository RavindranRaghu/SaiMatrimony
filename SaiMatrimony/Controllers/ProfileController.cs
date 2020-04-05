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
    [BasicUser("basic")]
    public class ProfileController : Controller
    {
        SaiMatrimonyDb db;

        public ProfileController(SaiMatrimonyDb dbContext)
        {
            db = dbContext;
        }


        public IActionResult ManageProfile([FromBody] ProfileDetails profileDetails)
        {
            try
            {

                string userId = AuthSession.GetUserId(HttpContext, "userId");
                UserBasic userBasic = db.UserBasic.Where(x => x.UserIdSystem == userId).FirstOrDefault();
                if (profileDetails.ProfileId == 0)
                {
                    profileDetails.ProfileUserId = Guid.NewGuid().ToString();
                    profileDetails.UpdatedDate = DateTime.UtcNow;
                    profileDetails.UpdatedByName = userBasic.FirstName + " " + userBasic.MiddleName + " " + userBasic.LastName;
                    profileDetails.UpdatedById = userId;

                    db.ProfileDetails.Attach(profileDetails);
                    db.ProfileDetails.Add(profileDetails);
                }
                else
                {
                    ProfileDetails profileDetailsDb = db.ProfileDetails.Where(x => x.ProfileId == profileDetails.ProfileId).FirstOrDefault();
                    profileDetailsDb.FirstName = profileDetails.FirstName;
                    profileDetailsDb.MiddleName = profileDetails.MiddleName;
                    profileDetailsDb.LastName = profileDetails.LastName;
                    profileDetailsDb.Gender = profileDetails.Gender;
                    profileDetailsDb.Email = profileDetails.Email;
                    profileDetailsDb.Phone = profileDetails.Phone;
                    profileDetailsDb.City = profileDetails.City;
                    profileDetailsDb.StateName = profileDetails.StateName;
                    profileDetailsDb.Country = profileDetails.Country;
                    profileDetailsDb.ZipCode = profileDetails.ZipCode;
                    profileDetailsDb.Education = profileDetails.Education;
                    profileDetailsDb.Profession = profileDetails.Profession;
                    profileDetailsDb.Interest = profileDetails.Interest;
                    profileDetailsDb.Expectation = profileDetails.Expectation;
                    profileDetailsDb.YearOfBirth = profileDetails.YearOfBirth;
                    profileDetailsDb.UpdatedDate = DateTime.UtcNow;
                    profileDetails.UpdatedByName = userBasic.FirstName + " " + userBasic.MiddleName + " " + userBasic.LastName;
                    profileDetails.UpdatedById = userId;
                }
                db.SaveChanges();
                return Json("y");
            }
            catch (Exception)
            {
                return Json("n");
            }
        }

    }
}