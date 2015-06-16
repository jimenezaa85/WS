using System.Linq;
using TaxRay.Model;

namespace TaxRay.Tests.Helpers
{
    public class FakeTaxReturnSet : FakeDbSet<TaxReturn>
    {
        public override TaxReturn Find(params object[] keyValues)
        {
            return this.SingleOrDefault(t => t.Id == (int)keyValues.Single());
        }
    }
}