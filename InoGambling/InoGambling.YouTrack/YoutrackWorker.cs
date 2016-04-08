using System;
using InoGambling.CommonMessages.Commands;
using InoGambling.Framework;
using InoGambling.Framework.Intergations.Trackers;
using NServiceBus;
using C = InoGambling.Framework.BeautifulConstants;
namespace InoGambling.YouTrack
{
    public class YoutrackWorker
        : IYouTrackIntegration
    {

        private IBus _bus;
        private Address _coreAddress;

        public YoutrackWorker(IBus bus)
        {
            _bus = bus;
            _coreAddress = new Address(C.CoreEndpoint, C.MachineName);
        }

        public void Work()
        {
            Console.WriteLine("Sending...");
            //TODO: fix stab method. 
            _bus.Send(new Address(C.CoreEndpoint, C.MachineName), new SimpleCommand
            {
                SomeShit = "Some piece of working shit. Really. Working nice and good. Ehehehe"
            });
        }

        public void Dispose()
        {
            //todo: dispose something. Like.. really. Dispose something.
        }


    }
}
