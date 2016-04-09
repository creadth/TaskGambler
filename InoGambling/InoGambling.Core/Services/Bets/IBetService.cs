using System;
using System.Threading.Tasks;
using InoGambling.Core.Services.Bets.Models;
using InoGambling.Data.Models;

namespace InoGambling.Core.Services.Bets
{
    public interface IBetService
    {
        Task<MakeBetResult> MakeBet(
            Int64 userId, 
            Int64 taskId, 
            Double estimate, 
            Double points);

        Task<MakeBetResult> MakeBet(
            IntegrationType integration, 
            String userName, 
            String taskShortId, 
            Double estimate, 
            Double points);

        Task<CancelBetResult> CancelBet(
            Int64 betId);

        Task<CancelBetResult> CancelBet(
            IntegrationType integration,
            String userName,
            String taskShortId);
    }
}
