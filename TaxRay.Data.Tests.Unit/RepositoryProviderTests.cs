using System;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TaxRay.Contracts;
using TaxRay.Data.Helpers;
using MSTestExtensions;
using TaxRay.Model;
using TaxRay.Tests.Helpers;

namespace TaxRay.Data.Tests.Unit
{
    [TestClass]
    public class RepositoryProviderTests
    {
        [TestMethod]
        public void GetRepositoryForEntityType_CallsFactory()
        {
            var mockContext = new Mock<DbContext>();
            Func<DbContext, object> inst1 = o => new EntityFrameworkRepository<ITaxReturnRepository>(mockContext.Object);
            var mockFactory = new Mock<RepositoryFactories>();
            mockFactory.Setup(m => m.GetRepositoryFactoryForEntityType<ITaxReturnRepository>())
                .Returns(inst1)
                .Verifiable();

            var provider = new RepositoryProvider(mockFactory.Object);
            provider.GetRepositoryForEntityType<ITaxReturnRepository>();

            mockFactory.Verify(m => m.GetRepositoryFactoryForEntityType<ITaxReturnRepository>(), Times.AtLeastOnce);            
        }

        [TestMethod]
        public void GetRepositoryForEntityType_EmptyFactories_Throws()
        {
            var mockFactory = new Mock<RepositoryFactories>();

            var provider = new RepositoryProvider(mockFactory.Object);
            ThrowsAssert.Throws<NotImplementedException>(() 
                    => provider.GetRepositoryForEntityType<ITaxReturnRepository>(), 
                    "No factory for repository type, TaxRay.Contracts.IRepository`1[[TaxRay.Contracts.ITaxReturnRepository, TaxRay.Contracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]");

            mockFactory.Verify(m => m.GetRepositoryFactoryForEntityType<ITaxReturnRepository>(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetRepository_OneElementInBag_ReturnsSameElement()
        {
            Func<DbContext, object> inst = c=> new TaxReturnRepository(new FakeTaxRayContext());
            var dic = new Dictionary<Type, Func<DbContext, object>> {{typeof (TaxReturn), inst}};
            var factories = new RepositoryFactories(dic);

            var provider = new RepositoryProvider(factories);
            var repository = provider.GetRepositoryForEntityType<TaxReturn>();

            Assert.IsInstanceOfType(repository, typeof(ITaxReturnRepository));
        }

        [TestMethod]
        public void SetRepository_OneElementInBag_ReturnsSameElement()
        {
            var context = new FakeTaxRayContext();
            var repo = new TaxReturnRepository(context);
            //var factories = new RepositoryFactories();
            //var provider = new RepositoryProvider(factories) {DbContext = context};
            var provider = DataHelper.CreateRepositoryProvider();
            provider.SetRepository<ITaxReturnRepository>(repo);

            var repository = provider.GetRepository<ITaxReturnRepository>();

            Assert.IsInstanceOfType(repository, typeof(EntityFrameworkRepository<TaxReturn>));
        }
    }
}