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
        :IHandleMessages<BetResponse>
    {

        private IBus _bus;
        private ISlackIntegration _bot;

        public BetResponseHandler(IBus bus, ISlackIntegration bot)
        {
            _bot = bot;
            _bus = bus;
        }

        public void Handle(BetResponse message)
        {
#if DEBUG
            Console.WriteLine($"Received bet command response from user <@{message.UserId}>");
#endif
            if (message.IsOk)
            {
                _bot.SendMessage($"Your bet was accepted, {message.AdditionalMessage}. Good estimating.", message.UserId);
            }
            else
            {
                _bot.SendMessage(
                    $"Your bet was not accepted because ~evil cucumber not gonna like you~ {message.AdditionalMessage}",
                    message.UserId);
            }
        }
    }
}
