using System;
using System.Collections.Generic;

namespace InoGambling.Data.Model
{
    public class Project : EntityBase
    {
        public virtual String ShortId { get; set; }
        public virtual String ProjectName { get; set; }
        public virtual IntegrationType IntegrationType { get; set; }

        public virtual Boolean IsArchive { get; set; }
        public virtual Boolean IsForbidden { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
