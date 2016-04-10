using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InoGambling.CommonMessages.Commands.Integrations.Slack;
using InoGambling.Framework.Intergations.Messengers;
using InoGambling.Slack.Helpers;
using NServiceBus;
using C = InoGambling.Framework.BeautifulConstants;
namespace InoGambling.Slack.Handlers
{
    public class UserManagementHandler
        : IHandleMessages<RegisterResult>,
            IHandleMessages<StatsCommand>
    {

        public ISlackIntegration _bot;

        public UserManagementHandler(ISlackIntegration bot)
        {
            _bot = bot;
        }

        public void Handle(RegisterResult message)
        {
            _bot.SendMessage(
                message.RegOk
                    ? $"{message.UserId.ToSlackMention()}, you were successfully registered. You've been rewarded with {C.StartingPoints} starting points. Good estimating!"
                    : $"{message.UserId.ToSlackMention()}, unfortunately registration failed. ~Evil cucumber is the fault~ Actually, {message.AdditionalMessage}",
                message.UserId);
        }

        public void Handle(StatsCommand message)
        {
            _bot.SendMessage(
                message.IsOk
                    ? $"{message.UserId.ToSlackMention()}, you have {message.Points} Points. Good estimating!"
                    : $"{message.UserId.ToSlackMention()}, {message.AdditionalMessage}",
                message.UserId);
        }
    }
}
