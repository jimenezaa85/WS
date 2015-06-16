using System;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TaxRay.Contracts;
using TaxRay.Data.Helpers;

namespace TaxRay.Data.Tests.Unit
{
    [TestClass]
    public class RepositoryFactoriesTests
    {
        [TestMethod]
        public void GetRepositoryFactory_DefaultContructor_ReturnValidInstance()
        {
            var factory = new RepositoryFactories();
            var repositories = factory.GetRepositoryFactory<ITaxReturnRepository>();

            Assert.IsNotNull(repositories);
        }

        [TestMethod]
        public void GetRepositoryFactoryForEntityType_DefaultContructor_ReturnValidInstance()
        {
            var factory = new RepositoryFactories();
            var repositories = factory.GetRepositoryFactoryForEntityType<ITaxReturnRepository>();

            Assert.IsNotNull(repositories);
        }

        [TestMethod]
        public void GetRepositoryFactory_SuppliedContructor_Used()
        {
            Func<DbContext, object> inst1;
            var mock = new Mock<IDictionary<Type, Func<DbContext, object>>>();
            mock.Setup(m=> m.TryGetValue(It.IsAny<Type>(),out inst1)).Verifiable();


            var factory = new RepositoryFactories(mock.Object);
            factory.GetRepositoryFactory<ITaxReturnRepository>();

            mock.Verify(m => m.TryGetValue(It.IsAny<Type>(), out inst1), Times.AtLeastOnce);
        }

        private class FakeRepositoryFactories : RepositoryFactories
        {
            public Func<DbContext, object> GetTaxReturnRepository()
            {
                return DefaultEntityRepositoryFactory<ITaxReturnRepository>();
            }
        }

        [TestMethod]
        public void DefaultEntityRepositoryFactory_DefaultContructor_ReturnValidInstance()
        {
            var factory = new FakeRepositoryFactories();
            var repositories = factory.GetTaxReturnRepository();

            Assert.IsNotNull(repositories);
        }
    }
}
