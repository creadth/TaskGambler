using InoGambling.CommonMessages.Commands;
using InoGambling.Framework.Intergations.Messengers;
using NServiceBus;
using SlackAPI;
using System;
using InoGambling.CommonMessages.Commands.Integrations.Slack;
using C = InoGambling.Framework.BeautifulConstants;

namespace InoGambling.Slack
{
    public class Bot : ISlackIntegration
    {
        private IBus _bus;
        private Address _coreAddress;

       private SlackSocketClient client;

        public Bot(IBus bus)
        {
            _bus = bus;
            _coreAddress = new Address(C.CoreEndpoint, C.MachineName);
        }

        public void Work()
        {
            client = new SlackSocketClient("xoxb-33255991907-GIsd17eRb8bMgLuVMrm4Uv8H");
            client.Connect((connected) => {
                //This is called once the client has emitted the RTM start command        
                

            }, () => {
                //This is called once the RTM client has connected to the end point
                client.OnHello += Client_OnHello;
                
                client.OnMessageReceived += Client_OnMessageReceived;                 
                                
            });
        }

        private void Client_OnHello()
        {
            _bus.Send(_coreAddress, new SimpleCommand
            {
                SomeShit = "Hello!"
            });
        }

        private void Client_OnMessageReceived(SlackAPI.WebSocketMessages.NewMessage obj)
        {
            //проверить, что чувак не поставил на себя
            if (obj.text.Contains(C.BetWord))
            {
                Int64 id;
                Double es;
                Double pt;

                Int64.TryParse(obj.text.Split( ' ', ',', '.', ':', '\t' )[1], out id);
                Double.TryParse(obj.text.Split( ' ', ',', '.', ':', '\t' )[2], out es);
                Double.TryParse(obj.text.Split(' ', ',', '.', ':', '\t' )[3], out pt);
              
                _bus.Send(_coreAddress, new BetCommand
                {
                    TicketId = id,
                    Estimate = es,
                    Points = pt//,
                    //UserId = obj.user как передат юзера?    
                });
            }
            if (obj.text.Contains(C.Want))
            {
                _bus.Send(_coreAddress, new WantBet
                {
                    Initiator = obj.user   
                });
            }
        }

        /// <summary>
        /// Post available tasks to bet for bet
        /// </summary>
        /// <param name="message"></param>
        public void SendTasks(BetCommandResponse message)
        {
            foreach (var task in message.Tasks)
            {
                client.PostMessage(null, message.Initiator, task.Id.ToString(), "Bot", null, false, null, false,
                     null, null, true);
            }
        }

        public void TaskNotification(Tasks notification)
        {
            foreach (var task in notification.TaskNotification)
            {
                client.PostMessage(null, "#general", task.Id.ToString(), "Bot", null, false, null, false,
                     null, null, true);
            }
        } 

        public void Dispose()
        {
            //todo: dispose something. Like.. really. Dispose something.
        }
    }
}
