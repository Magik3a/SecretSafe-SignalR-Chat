using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SecretSafe
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
             name: "Default2",
             url: "{lang}/NormalSecurity/{id}",
             constraints: new { lang = @"(\w{2})|(\w{2}-\w{2})"},
             defaults: new { controller = "NormalSecurity", action = "Index", id = UrlParameter.Optional }
         );
            routes.MapRoute(
          name: "Default3",
          url: "{lang}/MediumSecurity/{id}",
             constraints: new { lang = @"(\w{2})|(\w{2}-\w{2})" },
          defaults: new { controller = "MediumSecurity", action = "Index", id = UrlParameter.Optional }
      );
            routes.MapRoute(
          name: "Default4",
          url: "{lang}/ProSecurity/{id}",
             constraints: new { lang = @"(\w{2})|(\w{2}-\w{2})" },
          defaults: new { controller = "ProSecurity", action = "Index", id = UrlParameter.Optional }
      );
            routes.MapRoute(
          name: "Default5",
          url: "{lang}/MaximumSecurity/{id}",
             constraints: new { lang = @"(\w{2})|(\w{2}-\w{2})" },
          defaults: new { controller = "MaximumSecurity", action = "Index", id = UrlParameter.Optional }
      );
            routes.MapRoute(
                name: "Default",
                url: "{lang}/{controller}/{action}/{id}",
             constraints: new { lang = @"(\w{2})|(\w{2}-\w{2})" },
                defaults: new { lang = "en", controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
