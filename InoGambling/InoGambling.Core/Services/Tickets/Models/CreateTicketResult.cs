using InoGambling.Data.Model;

namespace InoGambling.Core.Services.Tickets.Models
{
    public class CreateTicketResult
    {
        public CreateTicketState State { get; set; }
        public Ticket Ticket { get; set; }
    }

    public enum CreateTicketState : byte
    {
        Ok = 0,
        TicketExists = 1,
        ProjectNotExists = 2,
        UserNotExists = 3,

        Error = 100,
    }
}
