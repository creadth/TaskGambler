using System;

namespace InoGambling.Data.Models
{
    public class Bet : EntityBase
    {
        public virtual Int64 UserId { get; set; }
        public virtual Int64 TaskId { get; set; }
        public virtual Double Estimate { get; set; }
        public virtual Double Points { get; set; }
    }
}
