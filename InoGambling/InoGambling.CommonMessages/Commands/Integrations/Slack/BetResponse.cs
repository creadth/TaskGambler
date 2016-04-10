using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace InoGambling.CommonMessages.Commands.Integrations.Slack
{
    public class BetResponse
        : ICommand
    {
        /// <summary>
        /// Represents integration user id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Represents eiter bet was accepted successfully
        /// </summary>
        public bool IsOk { get; set; }
        
        /// <summary>
        /// Additional message to desplay to user in case of error
        /// </summary>
        public string AdditionalMessage { get; set; }
    }
}
