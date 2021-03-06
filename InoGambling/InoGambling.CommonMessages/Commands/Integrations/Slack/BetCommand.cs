﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace InoGambling.CommonMessages.Commands.Integrations.Slack
{
    public class BetCommand
        :ICommand
    {
        /// <summary>
        /// Integration user id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Id of task user want to bet
        /// </summary>
        public string TaskShortId { get; set; }
        /// <summary>
        /// Indicates either user is betting agains assignee estimate
        /// </summary>
        public bool IsBetAgainst { get; set; }
    }
}
