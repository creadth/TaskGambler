using System.Collections.Generic;
using InoGambling.Data.Model;

namespace InoGambling.Core.Services.Tickets.Models
{
    public class UpdateTicketResult
    {
        public UpdateTicketState State { get; set; }
        public Ticket Ticket { get; set; }
        public IEnumerable<Bet> InvalidatedBets { get; set; }
    }

    public enum UpdateTicketState : byte
    {
        Ok = 0,
        TicketNotExists = 1,
        ProjectNotExists = 2,
        UserNotExists = 3,

        Error = 100,
    }
}
