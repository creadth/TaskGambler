using InoGambling.Data.Models;

namespace InoGambling.Core.Services.Tickets.Models
{
    public class CreateTicketResult
    {
        public CreateTicketState State { get; set; }
        public Ticket Task { get; set; }
    }

    public enum CreateTicketState : byte
    {
        Ok = 0,
        TicketExists = 1,
        ProjectNotExists = 2,
        UserNotExists = 3,
    }
}
