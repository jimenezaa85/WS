using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TaxRay.Web.Tests.Unit
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class GlobalAsaxTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            RouteTable.Routes.Clear();

            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            //AreaRegistration.RegisterAllAreas();

            //routes.MapRoute(
            //    "default",
            //    "{controller}/{action}/{id}",
            //    new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }



        [TestMethod]
        public void Start_Calls_WebApi()
        {
            var asax = new FakeWebApiApplication();
            //var asax = MockRepository.GenerateDynamicMockWithRemoting<>() new FakeWebApiApplication();    
            asax.Launch();
        }
    }

    [ExcludeFromCodeCoverage]
    internal class FakeWebApiApplication : WebApiApplication
    {
        internal void Launch()
        {
            Application_Start();
        }
    }

}
