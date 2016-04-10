using System;

namespace InoGambling.Data.Model
{
    public class Bet : EntityBase
    {
        public virtual Int64 UserId { get; set; }
        public virtual Int64 TicketId { get; set; }
        public virtual Double Estimate { get; set; }
        public virtual Boolean IsAgree { get; set; }
        public virtual Double Points { get; set; }
        public virtual Boolean IsInvalidate { get; set; }
        public virtual Boolean IsCanceled { get; set; }
    }
}
