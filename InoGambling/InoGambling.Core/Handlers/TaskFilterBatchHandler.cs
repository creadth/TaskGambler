using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InoGambling.CommonMessages.Commands.Integrations;
using InoGambling.CommonMessages.Commands.Integrations.YouTrack;
using NServiceBus;

namespace InoGambling.Core.Handlers
{
    class TaskFilterBatchHandler
        : IHandleMessages<TaskFilterBatchCommand>
    {

        public void Handle(TaskFilterBatchCommand message)
        {
#if DEBUG
#endif
        }
    }
}
