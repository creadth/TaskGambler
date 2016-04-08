using System.Data.Entity.ModelConfiguration;
using InoGambling.Data.Models;

namespace InoGambling.Data.Configurations
{
    class IntegrationUserConfiguration : EntityTypeConfiguration<IntegrationUser>
    {
        public IntegrationUserConfiguration()
        {
            ToTable("IntegrationUser");

            HasKey(x => x.Id);

            Property(x => x.Name).HasMaxLength(256).IsRequired();
            Property(x => x.Type).IsRequired();
        }
    }
}
