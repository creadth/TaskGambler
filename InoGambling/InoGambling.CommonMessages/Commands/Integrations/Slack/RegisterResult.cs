using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace InoGambling.CommonMessages.Commands.Integrations.Slack
{
    /// <summary>
    /// Sent by Core back to Slack to represent status of reg operation
    /// </summary>
    public class RegisterResult
        : ICommand
    {
        public string UserId { get; set; }
        public bool RegOk { get; set; }
        public string AdditionalMessage { get; set; }
    }
}
