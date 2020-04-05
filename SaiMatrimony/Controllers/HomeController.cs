using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SaiMatrimony.Auth;
using SaiMatrimony.Dto;
using SaiMatrimony.Models;
using SaiMatrimony.ViewModel;

namespace SaiMatrimony.Controllers
{
    public class HomeController : Controller
    {
        SaiMatrimonyDb db; 

        public HomeController(SaiMatrimonyDb dbContext)
        {
            db = dbContext;
        }

        [BasicUser("basic")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult GetProfiles (string edu, string pro, string gen, string location, string category)
        {
            return Json(new ProfileDto().GetProfilesEndPoint(db, edu, pro, gen, location, category));
        }

        //No Authorization Set intentionally
        public IActionResult Login(int? id)
        {
            id = (id == null) ? 0 : id;
            string nav = AuthSession.GetNav(HttpContext);
            ViewBag.id = id;
            ViewBag.nav = nav;
            return View();
        }

        //No Authorization Set intentionally
        public IActionResult getInExisting([FromBody] ExistingUser existingUser)
        {
            Hashing sai = new Hashing();
            existingUser.epd = sai.HashingPlain(existingUser.epd);
            string msg = "Bad User Name or Password";
            string success = "n";
            var hasUser = db.UserBasic.Where(x => x.Email == existingUser.eud);
            if (hasUser.Any())
            {
                UserBasic user = hasUser.FirstOrDefault();
                bool map = db.UserRoleMap.Any(x => x.UserIdSystem == user.UserIdSystem);
                if (user.PdSystem == existingUser.epd && map)
                {
                    AuthSession.SetUserId(HttpContext, user.UserIdSystem);
                    success = "y";
                    msg = "Authorization Complete";
                }
            }

            var result = new { success = success, msg = msg };
            return Json(result);
        }

        //No Authorization Set intentionally
        public IActionResult getInNew([FromBody] NewUser newUser)
        {
            Hashing sai = new Hashing();
            newUser.pd = sai.HashingPlain(newUser.pd);
            var result = new { success = true, user = newUser };
            return Json(result);
        }

        //No Authorization Set intentionally
        public IActionResult getInNewuser([FromBody] UserBasic newUser)
        {
            Hashing sai = new Hashing();
            newUser.PdSystem = sai.HashingPlain(newUser.PdSystem);
            newUser.UserIdSystem = Guid.NewGuid().ToString();
            string msg = string.Empty;
            bool success = false;
            UserBasic user = newUser;
            if (db.UserBasic.Where(x => x.Email.ToLower() == newUser.Email.ToLower()).Any())
            {
                msg = "User Already Exists. Please contact the Admin";
            }
            else
            {
                db.UserBasic.Add(newUser);
                db.SaveChanges();
                success = true;
                msg = "Your Request has been submitted, Email confirmation will be sent up on approval. Thanks";
                AuthSession.SetUserId(HttpContext, newUser.UserIdSystem);
            }
            var result = new { success = success, msg = msg };
            return Json(result);
        }

        //No Authorization Set intentionally
        public IActionResult SignOut()
        {
            AuthSession.SetUserId(HttpContext, "invalid");
            return View();
        }

        //No Authorization Set intentionally
        public IActionResult checkSigned()
        {
            string key = "userId";
            bool hasValue = false;
            UserBasic user = new UserBasic();
            if (HttpContext.Session.Get(key) != null)
            {
                hasValue = System.Text.Encoding.Default.GetString(HttpContext.Session.Get(key)) == "invalid" ? false : true;
                var hasuser = db.UserBasic.Where(x => x.UserIdSystem == System.Text.Encoding.Default.GetString(HttpContext.Session.Get(key)));
                if (hasuser.Any())
                {
                    user = db.UserBasic.Where(x => x.UserIdSystem == System.Text.Encoding.Default.GetString(HttpContext.Session.Get(key))).FirstOrDefault();
                    user.PdSystem = "";
                    user.UserIdSystem = "";
                }
            }
            var result = new { hasValue = hasValue, User = user };
            return Json(result);
        }


    }
}
