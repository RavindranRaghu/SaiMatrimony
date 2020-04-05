﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SaiMatrimony.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaiMatrimony.Auth
{
    
    public class UserAuthorize : IAuthorizationHandler
    {
        IHttpContextAccessor _httpContextAccessor = null;

        public UserAuthorize(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;

            var userId = AuthSession.GetUserId(httpContext, "userId");

            //context.Succeed();

            //TODO: Use the following if targeting a version of
            //.NET Framework older than 4.6:
            //      return Task.FromResult(0);
            return Task.CompletedTask;
        }
    }

    public class BasicUserAttribute : TypeFilterAttribute
    {
        public BasicUserAttribute(string permission) : base(typeof(UserFilter))
        {
            Arguments = new object[] { permission };
        }
    }

    public class UserFilter : IAuthorizationFilter
    {
        string _permission;
        private SaiMatrimonyDb db;

        public UserFilter(string pemission, SaiMatrimonyDb dbContext)
        {
            _permission = pemission;
            db = dbContext;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = AuthSession.GetUserId(context.HttpContext, "userId");
            var hasClaim = false;

            string path = context.HttpContext.Request.Path;
            AuthSession.SetNav(context.HttpContext, path);

            hasClaim = (from user in db.UserBasic
                        join map in db.UserRoleMap on user.UserIdSystem equals map.UserIdSystem
                        where user.UserIdSystem == userId 
                        select user
                        ).Any();

            if (!hasClaim)
            {
                context.Result = new RedirectToActionResult("Login", "Home", new { id = 1 });
            }
        }

    }
}
