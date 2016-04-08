using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InoGambling.Framework;
using Microsoft.Practices.Unity;
using NServiceBus;

namespace InoGambling.Core
{
    public class ConfigureEndpoint : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UseTransport<MsmqTransport>();
            configuration.EndpointName(BeautifulConstants.CoreEndpoint);
            configuration.EnableInstallers();
            configuration.DiscardFailedMessagesInsteadOfSendingToErrorQueue();
            configuration.UsePersistence<InMemoryPersistence>();
            //adding Unity
            var container = new UnityContainer();
            //configure dependencies here
            //---------^_^------
            configuration.UseContainer<UnityBuilder>(c => c.UseExistingContainer(container));
        }
    }
}
