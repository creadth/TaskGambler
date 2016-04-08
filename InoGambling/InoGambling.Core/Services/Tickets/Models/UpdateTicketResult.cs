using InoGambling.Data.Models;

namespace InoGambling.Core.Services.Tickets.Models
{
    class UpdateTicketResult
    {
        public UpdateTicketState State { get; set; }
        public Ticket Task { get; set; }
    }

    public enum UpdateTicketState : byte
    {
        Ok = 0,
        TicketNotExists = 1,
        ProjectNotExists = 2,
        UserNotExists = 3,
    }
}
