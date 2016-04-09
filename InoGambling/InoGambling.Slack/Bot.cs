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

        public Bot(IBus bus)
        {
            _bus = bus;
            _coreAddress = new Address(C.CoreEndpoint, C.MachineName);
        }

        public void Work()
        {            
            SlackSocketClient client = new SlackSocketClient("xoxb-33255991907-GIsd17eRb8bMgLuVMrm4Uv8H");
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

            //Bot.PostMessage(null, "#general", "Party Hard", "Bot", null, false, null, false,
            //     null, null, true);
        }

        private void Client_OnMessageReceived(SlackAPI.WebSocketMessages.NewMessage obj)
        {
            if (obj.text.Contains(C.BetWord))
            {
                Int64 id;
                Double es;
                Double pt;

                Int64.TryParse(obj.text.Split(new Char[] { ' ', ',', '.', ':', '\t' })[1], out id);
                Double.TryParse(obj.text.Split(new Char[] { ' ', ',', '.', ':', '\t' })[2], out es);
                Double.TryParse(obj.text.Split(new Char[] { ' ', ',', '.', ':', '\t' })[3], out pt);
                _bus.Send(_coreAddress, new BetCommand
                {
                    TicketId = id,
                    Estimate = es,
                    Points = pt    
                });
            }
        }

        public void Dispose()
        {
            //todo: dispose something. Like.. really. Dispose something.
        }
    }
}
