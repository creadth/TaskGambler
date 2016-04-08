using System.Data.Entity.ModelConfiguration;
using InoGambling.Data.Models;

namespace InoGambling.Data.Configurations
{
    class TaskConfiguration : EntityTypeConfiguration<Task>
    {
        public TaskConfiguration()
        {
            ToTable("Task");

            HasKey(x => x.Id);

            Property(x => x.ShortId).HasMaxLength(256).IsRequired();
            Property(x => x.State).IsRequired();
            Property(x => x.StartDate).IsOptional();
            Property(x => x.EndDate).IsOptional();
            Property(x => x.Estimate).IsRequired();
            Property(x => x.ExecutionTime).IsOptional();

            HasMany(x => x.Bets).WithRequired().HasForeignKey(x => x.TaskId).WillCascadeOnDelete(false);
        }
    }
}
