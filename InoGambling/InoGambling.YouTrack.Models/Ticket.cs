using System;

namespace InoGambling.YouTrack.Models
{
    public class Ticket
    {
        public string ShortId { get; set; }
        public string ProjectShortId { get; set; }
        public string State { get; set; }
        public string AssigneeName { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
