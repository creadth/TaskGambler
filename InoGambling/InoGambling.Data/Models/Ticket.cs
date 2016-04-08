using System;
using System.Collections.Generic;

namespace InoGambling.Data.Models
{
    public class Ticket : EntityBase
    {
        public virtual String ShortId { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual TicketState State { get; set; }
        public virtual TimeSpan? ExecutionTime { get; set; }
        public virtual Double Estimate { get; set; }
        public virtual IntegrationType IntegrationType { get; set; }

        public virtual Int64 AssigneeUserId { get; set; }

        public virtual Int64 ProjectId { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }

    }
}
