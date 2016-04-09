using System;
using NServiceBus;

namespace InoGambling.CommonMessages.Commands.Integrations.Slack
{
    /// <summary>
    /// User wants make a bet
    /// </summary>
    public class BetCommand: ICommand
    {
        public Int64 TicketId { get; set; }

        public Double Estimate { get; set; }

        public Double Points { get; set; }
    }
}
