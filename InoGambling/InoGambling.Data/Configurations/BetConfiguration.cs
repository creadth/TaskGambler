﻿using System.Data.Entity.ModelConfiguration;
using InoGambling.Data.Models;

namespace InoGambling.Data.Configurations
{
    class BetConfiguration : EntityTypeConfiguration<Bet>
    {
        public BetConfiguration()
        {
            ToTable("Bet");

            HasKey(x => x.Id);

            Property(x => x.Points).IsRequired();
            Property(x => x.Estimate).IsRequired();
        }
    }
}