using System;
using System.Threading.Tasks;
using InoGambling.Core.Services.Tickets.Models;
using InoGambling.Data.Models;

namespace InoGambling.Core.Services.Tickets
{
    public interface ITaskService
    {
        Task<CreateTicketResult> CreateTicket(
            IntegrationType integration,
            String projectShortId,
            String ticketShortId,
            String userName,
            String estimation);

        Task<CreateTicketResult> UpdateTicket(
            IntegrationType integration,
            String ticketShortId,
            String userName,
            String estimation,
            TicketState state);


    }
}
