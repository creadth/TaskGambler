using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoGambling.CommonMessages.Commands
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
