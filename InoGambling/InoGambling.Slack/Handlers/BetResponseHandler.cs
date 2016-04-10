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
        :IHandleMessages<BetResponse>,
            IHandleMessages<TicketPlayFinished>
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

        public void Handle(TicketPlayFinished message)
        {
            var totalWonPoints = message.Results.Sum(x => Math.Max(0, x.AmtChange));
            var amtWinners = message.Results.Sum(x => x.HasWon ? 1 : 0);
            _bot.SendBroadcast(
                $"*Attention*! Ticket <{message.TicketLink}|{message.TicketId}> was verified! {totalWonPoints} prize points were distributed between {amtWinners} winners, check direct messages for the info. ");
            foreach (var r in message.Results)
            {
                var greet = r.HasWon ? "Congratulations! You've been right" : "I regret to say, you've been wrong";
                var amtGreet = r.HasWon ? "added to your account" :"kept your";
                _bot.SendMessage(
                    $"{greet} about <{message.TicketLink}|{message.TicketId}>. We have {amtGreet} {r.AmtChange} Points and your balance is currently {r.CurrentPoints}. Good luck next time!",
                    r.UserId);
            }
        }
    }
}
