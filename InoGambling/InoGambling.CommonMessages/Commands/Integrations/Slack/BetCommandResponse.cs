using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoGambling.CommonMessages.Commands.Integrations.Slack
{
    public class BetCommandResponse
    {
        /// <summary>
        /// Represents slack ID of user, initiated request
        /// </summary>
        public string Initiator { get; set; }

        /// <summary>
        /// TODO: add actual data instead of stab
        /// </summary>
        public string SomeResponse { get; set; }
    }
}
