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
            Console.WriteLine($"Integration {message.Integration} tickets received:");
            foreach (var ticket in message.Tickets)
            {
                Console.WriteLine($"{ticket.ShortId} ({ticket.ProjectShortId}) WIP by {ticket.AssigneeName} is {ticket.State} at {ticket.UpdatedTime}");
            }
#endif
        }
    }
}
