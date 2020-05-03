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
                    profileDetails.MappedToUserIdSystem = userId;

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
                return Json(new KeyValuePair<string, string>("y", "Profile Saved Successfully"));
            }
            catch (Exception)
            {
                return Json(new KeyValuePair<string, string>("n", "Error saving profile, contact support"));
            }
        }

        public IActionResult SetMatch(string fromid, string toid, string maction, int reviewid)
        {
            KeyValuePair<bool, string> sMatch = new ProfileWorkFlow().SetMatch(db, fromid, toid, maction, reviewid);
            return Json(new KeyValuePair<string, string>(sMatch.Key ?"y": "n", sMatch.Value));
        }

        public IActionResult addcomment(string fromid, string toid, string comment, int reviewid)
        {
            
            ProfileDetails fromUser = db.ProfileDetails.Where(x => x.ProfileUserId == fromid).FirstOrDefault();
            ProfileDetails toUser = db.ProfileDetails.Where(x => x.ProfileUserId == toid).FirstOrDefault();
            ProfileComment pcomment = new ProfileComment();
            CommonFunction common = new CommonFunction();
            pcomment.ProfileReviewId = reviewid;
            pcomment.CommentText = common.GetName(fromUser) +" says to " + common.GetName(toUser) + " - " +  comment;
            pcomment.CommentByUserName = common.GetName(fromUser);
            pcomment.CommentByUserId = fromUser.ProfileUserId;
            pcomment.CommentDate = DateTime.UtcNow;
            db.ProfileComment.Add(pcomment);
            db.SaveChanges();
            return Json("y");
        }

        public IActionResult MatchExist(string fromid, string toid, string maction)
        {
            int profileId = 0;
            var hasMatch = db.ProfileReview.Where(x => x.ProposedFromUserId == fromid && x.ProposedToUserId == toid);
            var hasReMatch = db.ProfileReview.Where(x => x.ProposedToUserId == fromid && x.ProposedFromUserId == toid);
            if (hasMatch.Any() )
            {
                profileId = hasMatch.Select(x=>x.ProfileReviewId).FirstOrDefault();
                return Json(new KeyValuePair<string, int>("y" , profileId));
            }            
            else if (hasReMatch.Any())
            {
                profileId = hasReMatch.Select(x => x.ProfileReviewId).FirstOrDefault();
                return Json(new KeyValuePair<string, int>("y", profileId));
            }
            return Json(new KeyValuePair<string, int>("n", 0));
        }

        public IActionResult MatchDetail(int reviewid, string fromid)
        {                        
            ProfileReview profile = new ProfileReview();
            ProfileDetails pdetail = new ProfileDetails();
            List<ProfileComment> comments = new List<ProfileComment>();
            bool iProposed = false;
            string profileId = "";

            var hasMatch = db.ProfileReview.Where(x => x.ProfileReviewId == reviewid);
            if (hasMatch.Any())
            {
                profile = hasMatch.FirstOrDefault();                
                profileId = profile.ProposedFromUserId == fromid ? profile.ProposedToUserId : profile.ProposedFromUserId;
                iProposed = profile.ProposedFromUserId == fromid ? true : false;
                pdetail = db.ProfileDetails.Where(x => x.ProfileUserId == profileId).FirstOrDefault();
                comments = db.ProfileComment.Where(x => x.ProfileReviewId == reviewid).OrderByDescending(x=>x.CommentDate).ToList();
            }

            ViewBag.profile = profile;
            ViewBag.iProposed = iProposed;
            ViewBag.comments = comments;
            ViewBag.fromid = fromid;
            ViewBag.toid = profileId;
            return View(pdetail);
        }

        public IActionResult Notification(string fromid)
        {
            
            List<ProfileReview> iprofiles = db.ProfileReview.Where(x => x.ProposedFromUserId == fromid || x.ProposedToUserId ==fromid ).ToList();            

            List<ProfileComment> comments = db.ProfileComment.ToList();

            List<ProfileComment> icomments = (from com in comments
                                              join pro in iprofiles on com.ProfileReviewId equals pro.ProfileReviewId
                                              select com
                                              ).OrderByDescending(x => x.CommentDate).Take(10).ToList();

            return Json(icomments);
        }

    }
}