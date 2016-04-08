namespace InoGambling.Core.Services.Bets.Models
{
    public class CancelBetResult
    {
        public CancelBetState State { get; set; }
    }

    public enum CancelBetState
    {
        Ok = 0,
        BetNotExists = 1,
        UserNotExists = 2,
        ProjectNotExists = 3,
        TaskNotExists = 4,
    }
}
