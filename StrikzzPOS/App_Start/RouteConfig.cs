using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StrikzzPOS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //   ProductType/Product/1/3

            routes.MapMvcAttributeRoutes();



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
               defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                 name: "email",
                 url: "{controller}/{action}/{email}",
                 defaults: new { controller = "Account", action = "ResetPassword", email = UrlParameter.Optional }
             );
        }
    }
}
