using System.Data.Entity;
using TaxRay.Model;

namespace TaxRay.Tests.Helpers
{
    public class FakeTaxRayContext : DbContext
    {
        public FakeTaxRayContext()
        {
            Taxes = new FakeTaxReturnSet();
        }
        public IDbSet<TaxReturn> Taxes { get; set; }

        public override int SaveChanges()
        {
            return 0;
        }
    }
}