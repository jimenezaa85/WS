﻿using System.Web.Mvc;
using System.Web.Routing;

namespace TaxRay.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
       }
    }
}
