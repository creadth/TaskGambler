using System.Data.Entity.ModelConfiguration;
using InoGambling.Data.Models;

namespace InoGambling.Data.Configurations
{
    class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("User");

            HasKey(x => x.Id);

            Property(x => x.Login).HasMaxLength(50).IsRequired();
            Property(x => x.Password).HasMaxLength(128).IsRequired();
            Property(x => x.Points).IsRequired();

            HasMany(x => x.Bets).WithRequired().HasForeignKey(x => x.UserId).WillCascadeOnDelete(false);
            HasMany(x => x.Tickets).WithRequired().HasForeignKey(x => x.AssigneeUserId).WillCascadeOnDelete(false);
            HasMany(x => x.IntegrationUsers).WithRequired().HasForeignKey(x => x.UserId).WillCascadeOnDelete(false);
        }
    }
}
