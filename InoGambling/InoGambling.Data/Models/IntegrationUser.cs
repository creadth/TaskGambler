using System;

namespace InoGambling.Data.Models
{
    public class IntegrationUser : EntityBase
    {
        public virtual String Name { get; set; }
        public virtual IntegrationType Type { get; set; }
        public virtual Int64 UserId { get; set; }
    }
}
