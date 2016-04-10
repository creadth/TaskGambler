using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace InoGambling.CommonMessages.Commands.Integrations.Slack
{
    public class TicketPlayFinished
        : ICommand
    {
        public string TicketId { get; set; }
        public string TicketLink { get; set; }
        public List<TicketResult> Results { get; set; }
    }

    public class TicketResult
    {
        public string UserId { get; set; }
        public bool HasWon { get; set; }
        public double AmtChange { get; set; }
        public double CurrentPoints { get; set; }
    }
}
