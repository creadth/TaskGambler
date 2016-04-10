using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using NServiceBus;

namespace InoGambling.CommonMessages.Commands.Integrations.Slack
{
    public class Leaderboard
        :ICommand
    {
        public List<LeaderboardEntry> Entries;
        public string UserId { get; set; }
    }

    public class LeaderboardEntry
    {
        public string UserId { get; set; }
        public double Points { get; set; }
    }
}
