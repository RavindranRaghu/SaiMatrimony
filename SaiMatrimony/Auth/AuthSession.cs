using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaiMatrimony.Auth
{
    public static class AuthSession
    {
        public static string GetUserId(HttpContext httpContext, string key)
        {
            string userId = "invalid";
            //httpContext.Session.SetString("userId", "1");

            if (httpContext.Session.Get(key) != null)
            {
                userId = System.Text.Encoding.Default.GetString(httpContext.Session.Get(key));
            }
            return userId;
        }

        public static void SetUserId(HttpContext httpContext, string value)
        {
            httpContext.Session.SetString("userId", value);
        }

        public static string GetNav(HttpContext httpContext)
        {
            string navTo = "/home/index";
            //httpContext.Session.SetString("userId", "1");

            if (httpContext.Session.Get("nav") != null)
            {
                navTo = System.Text.Encoding.Default.GetString(httpContext.Session.Get("nav"));
            }
            return navTo;
        }

        public static void SetNav(HttpContext httpContext, string value)
        {
            httpContext.Session.SetString("nav", value);
        }
    }
}
