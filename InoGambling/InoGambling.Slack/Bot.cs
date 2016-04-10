using InoGambling.CommonMessages.Commands;
using InoGambling.Framework.Intergations.Messengers;
using NServiceBus;
using SlackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using InoGambling.CommonMessages.Commands.Integrations.Slack;
using Microsoft.Practices.ObjectBuilder2;
using SlackAPI.WebSocketMessages;
using C = InoGambling.Framework.BeautifulConstants;

namespace InoGambling.Slack
{
    public class Bot : ISlackIntegration
    {
        
        private const string _slackToken = "xoxb-33255991907-GIsd17eRb8bMgLuVMrm4Uv8H";

        private readonly IBus _bus;
        private readonly Address _coreAddress;
       
        private SlackSocketClient client;

        public Bot(IBus bus)
        {
            _bus = bus;
            _coreAddress = new Address(C.CoreEndpoint, C.MachineName);
        }

        public void Work()
        {
            client = new SlackSocketClient(_slackToken);
            client.OnHello += Client_OnHello;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.Connect((c) => { });
        }

        private void Client_OnHello()
        {
        }

        private void Client_OnMessageReceived(NewMessage message)
        {
            //ignore self messages
            if (message.user == client.MyData.id) return;
            //accept commands only from private
            if (!client.DirectMessageLookup.ContainsKey(message.channel)) return;
            var selfMention = $"<@{client.MyData.id}>";
            var responseMention = $"<@{message.user}|Dude>";
            //what a day without trim? ^_^
            var cleanMessage = message.text.Trim();
            //all commands should start with bot mention
            if (!cleanMessage.StartsWith(selfMention)) return;
            cleanMessage = cleanMessage.Replace(selfMention, string.Empty);
            //MORE TRIMS FOR THE GOD OF TRIMS
            cleanMessage = cleanMessage.TrimStart(' ', ':');
            //parse commands
            var argStack = new Queue<string>();
            cleanMessage.Split(' ').Select(x => x.ToLower()).ForEach(argStack.Enqueue);
            var cmd = argStack.Dequeue();
            //TODO: I MADE A DUMB SWITCH. YOU WANNA BE SMART? MAKE SMART SWITCH. 
            //DUMB SWITCH :
            switch (cmd)
            {
                case "reg":
                    if (!AssertEnoughArguments(1, argStack.Count, responseMention, message.channel)) break;
                    _bus.Send(_coreAddress, new RegisterCommand
                    {
                        UserId = message.user,
                        UserName = client.UserLookup[message.user].name,
                        YouTrackLogin = argStack.Dequeue()
                    });
                    break;
                case "bet":
                    if (!AssertEnoughArguments(2, argStack.Count, responseMention, message.channel)) break;
                    var bcmd = new BetCommand
                    {
                        UserId = message.user,
                        TaskShortId = argStack.Dequeue()
                    };
                    var betForWord = argStack.Dequeue();
                    if (betForWord != "against" && betForWord != "for")
                    {
                        SendMessage($"Evil cucumber misguided you. Syntax for bet command is bet <taskID> <against|for>", message.channel);
                    }
                    bcmd.IsBetAgainst = betForWord == "against";
                    _bus.Send(_coreAddress, bcmd);
                    break;
                case "stat":
                    _bus.Send(_coreAddress, new StatsCommand
                    {
                        UserId = message.user
                    });
                    break;
                default:
                    SendMessage($"{responseMention}, me like dunno wassup. wut r u talkn` bout?", message.channel);
                    break;
            }

        }
        #region Divine methods

        protected bool AssertEnoughArguments(int desired, int actually, string uMention, string chan)
        {
            var res = desired <= actually;
            if (!res)
            {
                SendMessage(
                    $"{uMention}! A shadow of Mordor has fallen across the Midlands. The {desired} arguments were expected for this command, but evil minions stole {desired - actually} from you.. Find them, or we all are dommed. Sincerely yours, unknown bot.",
                    chan);
            }
            return res;
        }
        #endregion
        #region Nice and warm methods
        public void Dispose()
        {
        }

        public bool SendMessage(string message, string recepient)
        {
            //find correct slack endpoint
            Console.WriteLine($"Post: {message}");
            if (!client.IsConnected) return false;
            client.PostMessage(null, channelId: GetChanId(recepient), text:message, as_user:true);
            return true;
        }

        public bool SendBroadcast(string message)
        {
            Console.WriteLine($"Post: {message}");
            if (!client.IsConnected) return false;
            foreach (var chan in client.Channels.Where(c => c.is_channel && !c.is_general && c.is_member))
            {
                SendMessage(message, chan.id);
            }
            return true;
        }

        /// <summary>
        /// Get correct channel id from whatever you pass as recepient - channel id, direct message id, user id.
        /// </summary>
        /// <param name="recepient"></param>
        /// <returns></returns>
        protected string GetChanId(string recepient)
        {
            //I can voodoo. Kek.
            return client.DirectMessages.FirstOrDefault(x => x.user == recepient || x.id == recepient)?.id
                   ??
                   client.Channels.FirstOrDefault(x => x.id == recepient)?.id;
        }
        #endregion
    }
}
