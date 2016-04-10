using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using InoGambling.Core.Services.Bets.Models;
using InoGambling.Data.Model;

namespace InoGambling.Core.Services.Bets
{
    public interface IBetService
    {
        MakeBetResult MakeBet(
            Int64 userId, 
            Int64 ticketId, 
            Double estimate, 
            Double points,
            Boolean isAgree);

        MakeBetResult MakeBet(
            IntegrationType integrationType, 
            String userName, 
            String ticketShortId, 
            Double estimate, 
            Double points,
            Boolean isAgree);

        CancelBetResult CancelBet(
            Int64 betId);

        CancelBetResult CancelBet(
            IntegrationType integrationType,
            String userName,
            String ticketShortId);

        PlayTicketResult PlayTicket(Int64 ticketId);
    }
}
