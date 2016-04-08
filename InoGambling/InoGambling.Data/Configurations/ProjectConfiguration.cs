using System.Data.Entity.ModelConfiguration;
using InoGambling.Data.Models;

namespace InoGambling.Data.Configurations
{
    class ProjectConfiguration : EntityTypeConfiguration<Project>
    {
        public ProjectConfiguration()
        {
            ToTable("Project");

            HasKey(x => x.Id);

            Property(x => x.ShortId).HasMaxLength(256).IsRequired();
            Property(x => x.ProjectName).HasMaxLength(512).IsRequired();
            Property(x => x.IsArchive).IsRequired();
            Property(x => x.IsForbidden).IsRequired();

            HasMany(x => x.Tasks).WithRequired().HasForeignKey(x => x.ProjectId).WillCascadeOnDelete();
        }
    }
}
