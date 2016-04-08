using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InoGambling.CommonMessages.Commands;
using NServiceBus;

namespace InoGambling.Core.Handlers
{
    public class SimpleMessageHandler
        : IHandleMessages<SimpleCommand>
    {
        private IBus _bus;

        public SimpleMessageHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(SimpleCommand message)
        {
            Console.WriteLine("Received: " + message.SomeShit);
        }
    }
}
