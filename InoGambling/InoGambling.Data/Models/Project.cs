using System;
using System.Collections.Generic;

namespace InoGambling.Data.Models
{
    public class Project : EntityBase
    {
        public virtual String ShortId { get; set; }
        public virtual String ProjectName { get; set; }

        public virtual Boolean IsArchive { get; set; }
        public virtual Boolean IsForbidden { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
