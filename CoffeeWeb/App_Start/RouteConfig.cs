using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CoffeeWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "CheckOut",
                url: "checkout",
                defaults: new { controller = "ShoppingCart", action = "CheckOut", alias = UrlParameter.Optional },
                namespaces: new[] { "CoffeeWeb.Controllers" }
            );
            routes.MapRoute(
                name: "CheckoutSuccess",
                url: "checkoutSuccess",
                defaults: new { controller = "ShoppingCart", action = "CheckOutSuccess" }
            );
            routes.MapRoute(
                name: "CategoryProduct",
                url: "product_category/{alias}-{id}",
                defaults: new { controller = "Products", action = "ProductCategory", id = UrlParameter.Optional },
                namespaces: new[] { "CoffeeWeb.Controllers" }
            );
            routes.MapRoute(
                name: "detailProducts",
                url: "detail/{alias}-p{id}",
                defaults: new { controller = "Products", action = "Detail", alias = UrlParameter.Optional },
                namespaces: new[] { "CoffeeWeb.Controllers" }
            );
            routes.MapRoute(
                name: "Products",
                url: "product",
                defaults: new { controller = "Products", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "CoffeeWeb.Controllers" }
            );
            routes.MapRoute(
                name: "DetailNew",
                url: "{alias}-n{id}",
                defaults: new { controller = "News", action = "Details", alias = UrlParameter.Optional },
                namespaces: new[] { "CoffeeWeb.Controllers" }
            );
            routes.MapRoute(
                name: "NewList",
                url: "news/{alias}-n{id}",
                defaults: new { controller = "News", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "CoffeeWeb.Controllers" }
            );
            routes.MapRoute(
                name: "Aboutus",
                url: "about-us",
                defaults: new { controller = "Aboutus", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "CoffeeWeb.Controllers" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "CoffeeWeb.Controllers" }
            );
            
        }
    }
}
