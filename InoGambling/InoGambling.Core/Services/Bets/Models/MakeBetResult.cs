using InoGambling.Data.Models;

namespace InoGambling.Core.Services.Bets.Models
{
    public class MakeBetResult
    {
        public MakeBetState State { get; set; }
        public Bet Bet { get; set; }
    }

    public enum MakeBetState : byte
    {
        Ok = 0,
        AllBetsAreOff = 1,
        UserNotExists = 2,
        ProjectNotExists = 3,
        TaskNotExists = 4,
    }
}
