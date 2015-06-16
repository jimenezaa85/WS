using TaxRay.Data.Helpers;

namespace TaxRay.Tests.Helpers
{
    public class DataHelper
    {
        public static RepositoryProvider CreateRepositoryProvider()
        {
            var context = new FakeTaxRayContext();
            var factories = new RepositoryFactories();
            return new RepositoryProvider(factories) { DbContext = context };            
        }
    }
}
