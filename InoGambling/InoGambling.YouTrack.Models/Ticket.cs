using System;
using InoGambling.Data.Model;

namespace InoGambling.YouTrack.Models
{
    public class Ticket
    {
        public string ShortId { get; set; }
        public string ProjectShortId { get; set; }
        public TicketState State { get; set; }
        public string AssigneeName { get; set; }
        public string UpdaterName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int Estimation { get; set; }
        public string Link { get; set; }
        public string Summary { get; set; }
    }
}
