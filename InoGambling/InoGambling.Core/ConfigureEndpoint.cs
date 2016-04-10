using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InoGambling.Core.Services.Bets;
using InoGambling.Core.Services.Bets.Impl;
using InoGambling.Core.Services.Projects;
using InoGambling.Core.Services.Projects.Impl;
using InoGambling.Core.Services.Tickets;
using InoGambling.Core.Services.Tickets.Impl;
using InoGambling.Core.Services.Users;
using InoGambling.Core.Services.Users.Impl;
using InoGambling.Data;
using InoGambling.Data.Impl;
using InoGambling.Data.Repositories;
using InoGambling.Data.Repositories.Impl;
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
            container.RegisterType<IUnitOfWork, UnitOfWorkImpl>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDataContextFactory, DataContextFactoryImpl>(new ContainerControlledLifetimeManager());
            container.RegisterType<IBetRepository, BetRepositoryImpl>(new ContainerControlledLifetimeManager());
            container.RegisterType<IIntegrationUserRepository, IntegrationUserRepositoryImpl>(new ContainerControlledLifetimeManager());
            container.RegisterType<IProjectRepository, ProjectRepositoryImpl>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITicketRepository, TicketRepositoryImpl>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserRepository, UserRepositoryImpl>(new ContainerControlledLifetimeManager());
            container.RegisterType<IBetService, BetServiceImpl>(new ContainerControlledLifetimeManager());
            container.RegisterType<IProjectService, ProjectServiceImpl>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITicketService, TicketServiceImpl>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserService, UserServiceImpl>(new ContainerControlledLifetimeManager());
            //configure dependencies here
            //---------^_^------
            configuration.UseContainer<UnityBuilder>(c => c.UseExistingContainer(container));
        }
    }
}
