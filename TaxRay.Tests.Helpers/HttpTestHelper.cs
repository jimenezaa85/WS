using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using TaxRay.Web;

namespace TaxRay.Tests.Helpers
{
    [ExcludeFromCodeCoverage]
    public class HttpTestHelper
    {
        public static void SetupControllerForTests(ApiController controller, string url, string controllerName, string routeName, IIdentity identity = null)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", controllerName } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            //Mock user
            var claims = new List<ClaimsIdentity>();

            var claimsCollection = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, "TestUser")
                        };
            claims.Add(new ClaimsIdentity(claimsCollection));

            controller.User = new ClaimsPrincipal(claims);
        }
    }
}