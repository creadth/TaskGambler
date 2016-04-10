using System;
using InoGambling.Data.Model;

namespace InoGambling.Core.Services.Bets.Models
{
    public class PlayTicketResult
    {
        public Ticket Ticket;
        public PlayTicketUserResult AssigneeReesult;
        public PlayTicketUserResult[] PlayersResults;
    }

    public class PlayTicketUserResult
    {
        public User User { get; set; }
        public Boolean Win { get; set; }
        public Double PointsResult { get; set; }
    }
}
