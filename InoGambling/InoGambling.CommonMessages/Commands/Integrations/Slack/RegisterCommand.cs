using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InoGambling.Data.Model;
using NServiceBus;

namespace InoGambling.CommonMessages.Commands.Integrations.Slack
{
    public class RegisterCommand
        : ICommand
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string YouTrackLogin { get; set; }
        public bool YoutrackConfirmed { get; set; }
        public string YouTrackDisplayName { get; set; }
    }
}
