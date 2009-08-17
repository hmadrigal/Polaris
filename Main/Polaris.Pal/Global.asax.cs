﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Polaris.Bal;
using Polaris.Bal.Extensions;

namespace Polaris.Pal
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // This rule is for the captch.ashx?guid=... 
            routes.IgnoreRoute("{handler}.ashx/{*pathInfo}", new { handler = @"captcha" });

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

        }

        public static void RegisterPageSizes() {
            SiteSection.HomePage.RegisterPageSize<IGame>(9);
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
            RegisterPageSizes();
        }
    }
}