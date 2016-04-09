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
        Task<MakeBetResult> MakeBet(
            Int64 userId, 
            Int64 ticketId, 
            Double estimate, 
            Double points);

        Task<MakeBetResult> MakeBet(
            IntegrationType integrationType, 
            String userName, 
            String ticketShortId, 
            Double estimate, 
            Double points);

        Task<CancelBetResult> CancelBet(
            Int64 betId);

        Task<CancelBetResult> CancelBet(
            IntegrationType integrationType,
            String userName,
            String ticketShortId);
    }
}
