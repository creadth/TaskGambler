using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InoGambling.Data.Configurations;

namespace InoGambling.Data
{
    public class InoGamblingDbContext : DbContext
    {
        public InoGamblingDbContext() : base("name=InoGambling")
        {
            Database.SetInitializer(new InoGamblingDbInitializer());
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ProjectConfiguration());
            modelBuilder.Configurations.Add(new TicketConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new IntegrationUserConfiguration());
            modelBuilder.Configurations.Add(new BetConfiguration());
        }
    }
}
