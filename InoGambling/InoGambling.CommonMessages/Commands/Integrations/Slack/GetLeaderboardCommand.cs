using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace InoGambling.CommonMessages.Commands.Integrations.Slack
{
    public class GetLeaderboardCommand
        :ICommand
    {
        public string UserId { get; set; }
    }
}
