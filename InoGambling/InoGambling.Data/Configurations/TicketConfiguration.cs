﻿using System.Data.Entity.ModelConfiguration;
using InoGambling.Data.Models;

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
            Property(x => x.Estimate).IsRequired();
            Property(x => x.ExecutionTime).IsOptional();
            Property(x => x.IntegrationType).IsRequired();

            HasMany(x => x.Bets).WithRequired().HasForeignKey(x => x.TicketId).WillCascadeOnDelete(false);
        }
    }
}