﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Papyrus.Web
{
    using System.Web.DynamicData;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Create",
                url: "Document/{action}",
                defaults: new {controller = "Document", action = "Create"}    
            );

            routes.MapRoute(
                name: "Details",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Document", action = "Detail"}    
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index"}
            );
        }
    }
}
