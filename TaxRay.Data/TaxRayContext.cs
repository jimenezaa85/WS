using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TaxRay.Data.Configuration;
using TaxRay.Model;

namespace TaxRay.Data
{
    public class TaxRayContext : DbContext
    {
        public TaxRayContext()
            : base("TaxRayDb")
        {
        }
        public TaxRayContext(string conn)
            : base(conn)
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public IDbSet<TaxReturn> Taxes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();            

            modelBuilder.Configurations.Add(new TaxReturnConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
        }
    }
}