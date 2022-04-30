using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Line
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "home", action = "index", id = UrlParameter.Optional },
                namespaces: new[] { "Line.Controllers" }
            );

            routes.MapRoute(

               name: "login",
               url: "{controller}/{action}/{returnurl}",
               defaults: new { controller = "account", action = "login", returnurl = UrlParameter.Optional },
               namespaces: new[] { "Line.Controllers" }
           );

            routes.MapRoute(

               name: "logout",
               url: "{controller}/{action}",
               defaults: new { controller = "account", action = "logout" },
               namespaces: new[] { "Line.Controllers" }
           );

            routes.MapRoute(

                name: "ChangeLanguage",
                url: "{controller}/{action}/{langid}",
                defaults: new { controller = "Configuration", action = "SetLanguage", langid = UrlParameter.Optional },
                namespaces: new[] { "Line.Controllers" }
            );


            //routes.MapRoute("ChangeLanguage",
            //               "changelanguage/{langid}",
            //               new { controller = "Configuration", action = "SetLanguage" },
            //               new { langid = @"\d+" },
            //               new[] { "Line.Controllers" });

        }

    }
}
