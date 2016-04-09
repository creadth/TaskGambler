using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InoGambling.CommonMessages.Commands.Integrations;
using Microsoft.Practices.ServiceLocation;
using NServiceBus;
using C = InoGambling.Framework.BeautifulConstants;
namespace InoGambling.YouTrack.Handlers
{
    public class SyncTimeHandler
        : IHandleMessages<SyncTimeCommand>
    {

        public static bool SyncReceived = false;

        public IBus _bus;

        public SyncTimeHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(SyncTimeCommand message)
        {
            if (SyncReceived) return;
#if DEBUG
            Console.WriteLine($"Received sync time response. Time is {message.SyncTime}");
#endif
            // start running youtrack worker
            Task.Run(() =>
            {
                var syncTime = message.SyncTime;
                while (true)
                {
                    using (var worker = ServiceLocator.Current.GetInstance<YoutrackWorker>())
                    {
                        worker.Work(ref syncTime);
                    }
                    Thread.Sleep(C.TrackerSleepDelay);
                }
            });
            SyncReceived = true;

        }
    }
}
