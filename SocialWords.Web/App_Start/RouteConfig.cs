using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SocialWords.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("styles/{*pathInfo}");
            routes.IgnoreRoute("scripts/{*pathInfo}");
            routes.IgnoreRoute("fonts/{*pathInfo}");

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //    name: "Definition",
            //    url: "definition/{id}",
            //    defaults: new { controller = "Definition", action = "Index", id = UrlParameter.Optional }
            //);
            //routes.MapRoute(
            //    name: "Def",
            //    url: "def/{id}",
            //    defaults: new { controller = "Definition", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
