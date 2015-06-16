using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TaxRay.Tests.Helpers;

namespace TaxRay.Data.Tests.Unit
{
    [TestClass]
    public class TaxRayUowTests
    {
        [TestMethod]
        public void Dispose_CallsUowDispose()
        {
            var provider = DataHelper.CreateRepositoryProvider();
            var mockContext = new Mock<DbContext>(provider);
            mockContext.Setup(m=> m.Dispose()).Verifiable();
            var uow = new TaxRayUow(provider);

            uow.Dispose();

            mockContext.Verify(m => m.Dispose(), Times.AtLeastOnce);
        }
    }
}