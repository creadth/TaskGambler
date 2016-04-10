using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InoGambling.CommonMessages.Commands.Integrations.Slack;

namespace InoGambling.CommonMessages.Commands.Integrations.YouTrack
{
    /// <summary>
    /// Distibuted to youtrack register command 
    /// </summary>
    public class YoutrackDistributedRegisterCommand
    {
        public string YouTrackLogin { get; set; }
        public string RegIntegrationUserId { get; set; }
        public RegisterCommand LegacyCommand { get; set; }
    }
}
