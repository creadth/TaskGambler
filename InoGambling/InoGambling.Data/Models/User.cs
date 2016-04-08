using System;
using System.Collections.Generic;

namespace InoGambling.Data.Models
{
    public class User : EntityBase
    {
        public virtual String Login { get; set; }
        public virtual String Password { get; set; }

        public virtual Double Points { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<Bet> Bets { get; set; }
        public virtual ICollection<IntegrationUser> IntegrationUsers { get; set; }

    }
}
