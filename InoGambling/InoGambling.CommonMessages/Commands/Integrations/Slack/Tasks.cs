﻿using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoGambling.CommonMessages.Commands.Integrations.Slack
{
    public class Tasks: ICommand
    {
        public IEnumerable<Task> TaskNotification { get; set; }
    }
}
