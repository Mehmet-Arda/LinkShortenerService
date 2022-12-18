﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinkShortenerService.Filters
{
    public class AuthFilter : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["user"]==null)
            {
                filterContext.Result = new RedirectResult("/Home/Login");
            }

        }
    }
}