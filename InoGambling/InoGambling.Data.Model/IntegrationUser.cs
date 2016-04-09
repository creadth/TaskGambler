using System;

namespace InoGambling.Data.Model
{
    public class IntegrationUser : EntityBase
    {
        public virtual String Name { get; set; }
        public virtual String DisplayName { get; set; }
        public virtual IntegrationType Type { get; set; }
        public virtual Boolean IsForbidden { get; set; }
        public virtual Int64 UserId { get; set; }
    }
}
