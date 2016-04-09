using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InoGambling.CommonMessages.Commands.Integrations.Slack;
using InoGambling.Framework.Intergations.Messengers;
using NServiceBus;
using Microsoft.Practices.ServiceLocation;

namespace InoGambling.Slack.Handlers
{
    public class BetResponseHandler
        :IHandleMessages<BetCommandResponse>
    {

        private IBus _bus;
        private ISlackIntegration _bot;

        public BetResponseHandler(IBus bus, ISlackIntegration bot)
        {
            _bot = bot;
            _bus = bus;
        }

        public void Handle(BetCommandResponse message)
        {
#if DEBUG
            Console.WriteLine($"Received bet command response from user <{message.Initiator}>");
#endif
            using (var worker = ServiceLocator.Current.GetInstance<Bot>())
            {
                worker.SendTasks(message);
            }
        }
    }
}
