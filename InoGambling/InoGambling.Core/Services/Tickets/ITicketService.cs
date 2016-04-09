﻿using System;
using System.Threading.Tasks;
using InoGambling.Core.Services.Tickets.Models;
using InoGambling.Data.Model;

namespace InoGambling.Core.Services.Tickets
{
    public interface ITicketService
    {
        Task<Ticket> GetTicket(
            Int64 id);

        Task<Ticket> GetTicket(
            IntegrationType integrationType,
            String ticketShortId);

        Task<CreateTicketResult> CreateTicket(
            IntegrationType integrationType,
            String projectShortId,
            String ticketShortId,
            String userName,
            Double estimate,
            String link,
            DateTime createDate);

        Task<UpdateTicketResult> UpdateTicket(
            IntegrationType integrationType,
            String ticketShortId,
            String userName,
            Double estimate,
            TicketState state,
            String link,
            DateTime lastChangeDate,
            DateTime? startDate,
            DateTime? endDate);


    }
}
