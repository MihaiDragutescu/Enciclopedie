using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Enciclopedie
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
              name: "ViewCategories",
              url: "category/index",
              defaults: new { controller = "Category", action = "Index" }
          );

            routes.MapRoute(
               name: "ViewArticles",
               url: "article/index",
               defaults: new { controller = "Article", action = "Index" }
           );

            routes.MapRoute(
                name: "NewArticle",
                url: "article/new",
                defaults: new { controller = "Article", action = "New" }
            );

            routes.MapRoute(
               name: "EditArticle",
               url: "article/edit",
               defaults: new { controller = "Article", action = "Edit" }
           );

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
