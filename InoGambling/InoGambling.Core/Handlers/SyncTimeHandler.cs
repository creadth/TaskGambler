using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InoGambling.CommonMessages.Commands.Integrations;
using NServiceBus;
using C = InoGambling.Framework.BeautifulConstants;
namespace InoGambling.Core.Handlers
{
    public class SyncTimeHandler
        : IHandleMessages<SyncTimeCommand>
    {

        private IBus _bus;

        public SyncTimeHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(SyncTimeCommand message)
        {
#if DEBUG
            Console.WriteLine($"Sync message from Integration {message.Integration}");
#endif
            //TODO: get real sync time
            message.SyncTime = DateTime.Now.AddDays(-1);
            _bus.Send(new Address(C.YouTrackEndpoint, C.MachineName), message);
        }
    }
}
