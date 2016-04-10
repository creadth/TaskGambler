using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoGambling.CommonMessages.Commands.Integrations.Slack
{
    /// <summary>
    /// Occurs when bets are no more accepted.
    /// </summary>
    public class TicketBetsClosed
    {
        public string TicketShortId { get; set; }
        public string AssigneeName { get; set; }
        public string TaskSummary { get; set; }
        public double Estimation { get; set; }
        public string LinkToTask { get; set; }
        public TimeSpan Delay { get;set; }
    }
}
