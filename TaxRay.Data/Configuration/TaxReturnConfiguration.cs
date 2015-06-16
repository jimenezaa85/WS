using System.Data.Entity.ModelConfiguration;
using TaxRay.Model;

namespace TaxRay.Data.Configuration
{
    class TaxReturnConfiguration : EntityTypeConfiguration<TaxReturn>
    {
        public TaxReturnConfiguration()
        {
            Property(r => r.AtlasId)
                .HasMaxLength(20);

            Property(r => r.Description)
                .HasMaxLength(50);

            Property(r => r.Client)
                .HasMaxLength(50);

            Property(r => r.TaxPayer)
                .HasMaxLength(50);

            Property(r => r.Year);

            Property(r => r.DueDate);

            Property(r => r.Status)
                .HasMaxLength(50);

            HasRequired(r => r.AssignedTo);

        }
    }
}
