using System;
using System.Threading.Tasks;
using InoGambling.Core.Services.Tickets.Models;
using InoGambling.Data.Model;

namespace InoGambling.Core.Services.Tickets
{
    public interface ITicketService
    {

        DateTime GetSyncTime();

        Ticket GetTicket(
            Int64 id);

        Ticket GetTicket(
            IntegrationType integrationType,
            String ticketShortId);

        CreateTicketResult CreateTicket(
            IntegrationType integrationType,
            String projectShortId,
            String ticketShortId,
            String userName,
            Double estimate,
            String link,
            DateTime createDate,
            double points);

        UpdateTicketResult UpdateTicket(
            IntegrationType integrationType,
            String ticketShortId,
            String userName,
            Double estimate,
            TicketState state,
            String link,
            DateTime lastChangeDate,
            DateTime? startDate,
            DateTime? endDate);

        Boolean IsTicketAllBetsAreOff(Int64 ticketId);
        Boolean IsTicketAllBetsAreOff(IntegrationType integrationType, String ticketShortId);
        Boolean IsTicketAllBetsAreOff(Ticket ticket);

    }
}
