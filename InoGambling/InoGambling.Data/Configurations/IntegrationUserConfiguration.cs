using System.Data.Entity.ModelConfiguration;
using InoGambling.Data.Model;

namespace InoGambling.Data.Configurations
{
    class IntegrationUserConfiguration : EntityTypeConfiguration<IntegrationUser>
    {
        public IntegrationUserConfiguration()
        {
            ToTable("IntegrationUser");

            HasKey(x => x.Id);

            Property(x => x.Name).HasMaxLength(256).IsRequired();
            Property(x => x.DisplayName).HasMaxLength(256).IsOptional();
            Property(x => x.Type).IsRequired();
            Property(x => x.IsForbidden).IsRequired();
        }
    }
}
