using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace InoGambling.CommonMessages.Commands.Integrations.Slack
{
    public class StatsCommand
        : ICommand
    {
        public string UserId { get; set; }
        public double Points { get; set; }
        public bool IsOk { get; set; }
        public string AdditionalMessage { get; set; }
    }
}
