using System.Threading;
using System.Threading.Tasks;
using InoGambling.Framework.Intergations.Messengers;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NServiceBus;
using C = InoGambling.Framework.BeautifulConstants;

namespace InoGambling.Slack
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
            configuration.EndpointName(C.SlackEndpoint);
            configuration.DiscardFailedMessagesInsteadOfSendingToErrorQueue();
            configuration.UsePersistence<InMemoryPersistence>();
            //adding Unity
            var container = new UnityContainer();
            var locator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => locator);
            //configure dependencies here
            container.RegisterType<ISlackIntegration, Bot>(new ContainerControlledLifetimeManager());
            //---------^_^------
            configuration.UseContainer<UnityBuilder>(c => c.UseExistingContainer(container));
        }

        public void Start()
        {
            using (var worker = ServiceLocator.Current.GetInstance<Bot>())
            {
                worker.Work();
            }
        }

        public void Stop()
        {
            //make bad!
        }
    }
}
