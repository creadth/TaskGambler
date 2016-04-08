using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using InoGambling.CommonMessages.Commands;
using InoGambling.Framework;
using InoGambling.Framework.Intergations.Trackers;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NServiceBus;
using C = InoGambling.Framework.BeautifulConstants;

namespace InoGambling.YouTrack
{
    /// <summary>
    /// Main entry for NServiceBus configuration
    /// </summary>
    public class ConfigureEndpoint
        : IConfigureThisEndpoint, IWantToRunWhenBusStartsAndStops
    {

        /// <summary>
        /// Represents IBus instance
        /// </summary>
        private IBus _bus;

        public ConfigureEndpoint()
        {
            
        }

        public ConfigureEndpoint(IBus bus)
        {
            _bus = bus;
        }

        public void Customize(BusConfiguration configuration)
        {
            configuration.UseTransport<MsmqTransport>();
            configuration.EnableInstallers();
            configuration.EndpointName(C.YouTrackEndpoint);
            configuration.DiscardFailedMessagesInsteadOfSendingToErrorQueue();
            configuration.UsePersistence<InMemoryPersistence>();
            //adding Unity
            var container = new UnityContainer();
            var locator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => locator);
            //configure dependencies here
            container.RegisterType<IYouTrackIntegration, YoutrackWorker>();
            //---------^_^------
            configuration.UseContainer<UnityBuilder>(c => c.UseExistingContainer(container));
        }

        public void Start()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    using (var worker = ServiceLocator.Current.GetInstance<YoutrackWorker>())
                    {
                        worker.Work();
                    }
                    Thread.Sleep(C.TrackerSleepDelay);
                }
            });
        }

        public void Stop()
        {
            //make bad!
        }
    }
}
