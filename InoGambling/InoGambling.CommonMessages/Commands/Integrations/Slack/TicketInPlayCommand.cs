using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace InoGambling.CommonMessages.Commands.Integrations.Slack
{
    /// <summary>
    /// Goes to integration when specific ticket is assigned by user and bets can be accepted
    /// </summary>
    public class TicketInPlayCommand
        : ICommand
    {
        public string TicketShortId { get; set; }
        public string AssigneeName { get; set; }
        public string TaskSummary { get; set; }
        public double Estimation { get; set; }
        public string LinkToTask { get; set; }
        public int Points { get; set; }
    }
}
