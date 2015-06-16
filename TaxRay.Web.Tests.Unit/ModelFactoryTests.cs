using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxRay.Model;
using TaxRay.Web.Models;

namespace TaxRay.Web.Tests.Unit
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ModelFactoryTests
    {
        private ModelFactory CreateFactory()
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            var request = new HttpRequestMessage(HttpMethod.Get, "");
            request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, config);

            return new ModelFactory(request);
        }

        [TestMethod]
        public void CreateTask_UserNull_NotNullReturn()
        {
            var factory = CreateFactory();
            var user = new TaxReturn {Id = 1, Description = "ZZZ", Client = "John Smith"};

            var result = factory.Create(user);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateTask_UserNotNull_NotNullReturn()
        {
            var factory = CreateFactory();
            var user = new TaxReturn { Id = 1, Description = "ZZZ", Client = "John Smith", AssignedTo = new User(), CreatedBy = new User()};

            var result = factory.Create(user);

            Assert.IsNotNull(result);
        }

    }
}
