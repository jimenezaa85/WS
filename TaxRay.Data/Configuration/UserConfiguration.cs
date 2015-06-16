using System.Data.Entity.ModelConfiguration;
using TaxRay.Model;

namespace TaxRay.Data.Configuration
{
    class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            Property(r => r.Username)
                .IsRequired()
                .HasMaxLength(10);
        }
    }
}
