using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KhoaLuanTotNghiep
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Account Details",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "accountinfo", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Vocabulary Details",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Vocabulary", action = "details", id = UrlParameter.Optional, searchWord = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Vocabulary topic details",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Vocabulary", action = "deleteTopic", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Vocabulary deleteVocab",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Vocabulary", action = "DeleteTV", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Vocabulary index",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Vocabulary", action = "index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Vocabulary practice",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Vocabulary", action = "Practice", id = UrlParameter.Optional }
           );
            routes.MapRoute(
              name: "Vocabulary match",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Vocabulary", action = "match", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "Vocabulary wordCorrect",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Vocabulary", action = "wordCorrect", id = UrlParameter.Optional }
          );
           


        }
    }
}
