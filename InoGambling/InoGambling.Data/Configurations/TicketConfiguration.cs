using System.Data.Entity.ModelConfiguration;
using InoGambling.Data.Model;

namespace InoGambling.Data.Configurations
{
    class TicketConfiguration : EntityTypeConfiguration<Ticket>
    {
        public TicketConfiguration()
        {
            ToTable("Ticket");

            HasKey(x => x.Id);

            Property(x => x.ShortId).HasMaxLength(256).IsRequired();
            Property(x => x.State).IsRequired();
            Property(x => x.StartDate).IsOptional();
            Property(x => x.EndDate).IsOptional();
            Property(x => x.LastUpdateDate).IsRequired();
            Property(x => x.Estimate).IsRequired();
            Property(x => x.ExecutionTime).IsOptional();
            Property(x => x.IntegrationType).IsRequired();
            Property(x => x.Link).HasMaxLength(512).IsRequired();
            Property(x => x.Points).IsRequired();
            HasMany(x => x.Bets).WithRequired().HasForeignKey(x => x.TicketId).WillCascadeOnDelete(false);
        }
    }
}
