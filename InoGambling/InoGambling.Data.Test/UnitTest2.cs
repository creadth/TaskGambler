using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using InoGambling.Core.Services.Bets;
using InoGambling.Core.Services.Bets.Impl;
using InoGambling.Core.Services.Projects;
using InoGambling.Core.Services.Projects.Impl;
using InoGambling.Core.Services.Tickets;
using InoGambling.Core.Services.Tickets.Impl;
using InoGambling.Core.Services.Users;
using InoGambling.Core.Services.Users.Impl;
using InoGambling.Data.Impl;
using InoGambling.Data.Model;
using InoGambling.Data.Repositories;
using InoGambling.Data.Repositories.Impl;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InoGambling.Data.Test
{
    /// <summary>
    /// Summary description for UnitTest2
    /// </summary>
    [TestClass]
    public class UnitTest2
    {
        public UnitTest2()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ServiceTest1()
        {
            RegisterUnityContainer();

            ServiceTestASync().Wait();
        }

        protected async Task ServiceTestASync()
        {
            try
            {
                var projectService = ServiceLocator.Current.GetInstance<IProjectService>();
                var tmp = await projectService.GetProject(IntegrationType.Youtrack, "TST");
                var newProj1 = await projectService.CreateProject(IntegrationType.Youtrack, "TST");
                var newProj2 = await projectService.CreateProject(IntegrationType.Youtrack, "TST1");
            }
            catch (Exception e)
            {
                throw;
            }
            
        }



        protected void RegisterUnityContainer()
        {
            IUnityContainer myContainer = new UnityContainer();

            myContainer.RegisterType<IUnitOfWork, UnitOfWorkImpl>(new ContainerControlledLifetimeManager());
            myContainer.RegisterType<IDataContextFactory, DataContextFactoryImpl>(new ContainerControlledLifetimeManager());

            myContainer.RegisterType<IBetRepository, BetRepositoryImpl>(new ContainerControlledLifetimeManager());
            myContainer.RegisterType<IIntegrationUserRepository, IntegrationUserRepositoryImpl>(new ContainerControlledLifetimeManager());
            myContainer.RegisterType<IProjectRepository, ProjectRepositoryImpl>(new ContainerControlledLifetimeManager());
            myContainer.RegisterType<ITicketRepository, TicketRepositoryImpl>(new ContainerControlledLifetimeManager());
            myContainer.RegisterType<IUserRepository, UserRepositoryImpl>(new ContainerControlledLifetimeManager());

            myContainer.RegisterType<IBetService, BetServiceImpl>(new ContainerControlledLifetimeManager());
            myContainer.RegisterType<IProjectService, ProjectServiceImpl>(new ContainerControlledLifetimeManager());
            myContainer.RegisterType<ITicketService, TicketServiceImpl>(new ContainerControlledLifetimeManager());
            myContainer.RegisterType<IUserService, UserServiceImpl>(new ContainerControlledLifetimeManager());

            UnityServiceLocator locator = new UnityServiceLocator(myContainer);
            ServiceLocator.SetLocatorProvider(() => locator);
        }
    }
}
