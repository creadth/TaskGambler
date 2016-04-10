using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InoGambling.CommonMessages.Commands.Integrations;
using InoGambling.Core.Services.Tickets;
using NServiceBus;
using C = InoGambling.Framework.BeautifulConstants;
namespace InoGambling.Core.Handlers
{
    public class SyncTimeHandler
        : IHandleMessages<SyncTimeCommand>
    {

        private IBus _bus;
        private ITicketService _ticketService;
        public SyncTimeHandler(IBus bus, ITicketService ticketService)
        {
            _bus = bus;
            _ticketService = ticketService;
        }

        public void Handle(SyncTimeCommand message)
        {
#if DEBUG
            Console.WriteLine($"Sync message from Integration {message.Integration}");
#endif
            message.SyncTime = _ticketService.GetSyncTime().Result;
            _bus.Send(new Address(C.YouTrackEndpoint, C.MachineName), message);
        }
    }
}
