using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InoGambling.CommonMessages.Commands.Integrations.Slack;
using InoGambling.Framework.Intergations.Messengers;
using NServiceBus;

namespace InoGambling.Slack.Handlers
{
    public class TicketMessageHandler
        : IHandleMessages<TicketInPlayCommand>, IHandleMessages<TicketBetsClosed>
    {
        private IBus _bus;
        private ISlackIntegration _bot;

        public TicketMessageHandler(IBus bus, ISlackIntegration bot)
        {
            _bus = bus;
            _bot = bot;
        }

        public void Handle(TicketInPlayCommand message)
        {
            var span = TimeSpan.FromMinutes(message.Estimation);
            if (
                !_bot.SendBroadcast(
                    $"New task in game! {message.AssigneeName} has estimated <{message.LinkToTask}|{message.TicketShortId}> by {span.Hours + span.Days * 24 }h{span.Minutes}m. Reward points: {message.Points}"))
            {
                _bus.Defer(TimeSpan.FromSeconds(10), message);
            }
        }

        public void Handle(TicketBetsClosed message)
        {
            //throw new NotImplementedException();
        }
    }
}
