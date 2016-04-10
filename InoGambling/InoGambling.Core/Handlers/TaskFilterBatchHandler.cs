using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using InoGambling.CommonMessages.Commands.Integrations;
using InoGambling.CommonMessages.Commands.Integrations.Slack;
using InoGambling.CommonMessages.Commands.Integrations.YouTrack;
using InoGambling.Core.Services.Projects;
using InoGambling.Core.Services.Projects.Models;
using InoGambling.Core.Services.Tickets;
using InoGambling.Core.Services.Tickets.Models;
using InoGambling.Core.Services.Users;
using InoGambling.Core.Services.Users.Models;
using InoGambling.Data.Model;
using NServiceBus;
using C = InoGambling.Framework.BeautifulConstants;

namespace InoGambling.Core.Handlers
{
    class TaskFilterBatchHandler
        : IHandleMessages<TaskFilterBatchCommand>
    {

        public IBus _bus;
        public ITicketService _ticketService;
        public IProjectService _projectService;
        public IUserService _userService;

        public TaskFilterBatchHandler(IBus bus,
            ITicketService ticketService,
            IProjectService projectService,
            IUserService userService)
        {
            _bus = bus;
            _ticketService = ticketService;
            _projectService = projectService;
            _userService = userService;
        }

        public void Handle(TaskFilterBatchCommand message)
        {
#if DEBUG
            Console.WriteLine($"{message.Tickets.Count} Integration {message.Integration} tickets received");
#endif
            try {
                foreach (var ticket in message.Tickets)
                {
                   Ticket tryTicket = null;
                    //var tryTicket = _ticketService.GetTicket(IntegrationType.Youtrack, ticket.ShortId).Result;
                    //if (tryTicket == null )
                    {
                        if (ticket.AssigneeName == null) continue;
                        if (ticket.Estimation == 0) continue;
                        //if (_projectService
                        //    .CreateProject(IntegrationType.Youtrack, ticket.ProjectShortId)
                        //    .Result
                        //    .State == CreateProjectState.Error)
                        //{
                        //    continue;
                        //}
                        //var user = _userService.GetIntegrationUser(IntegrationType.Youtrack, ticket.AssigneeName);
                        //if (user == null)
                        //{
                        //    if (_userService.CreateIntegrationUser(null, ticket.AssigneeName, ticket.AssigneeName,
                        //        IntegrationType.Youtrack, false).Result.State == CreateIntegrationUserState.Error)
                        //    {
                        //        continue;
                        //    }
                        //}

                        //var res = _ticketService.CreateTicket(IntegrationType.Youtrack,
                        //    ticket.ProjectShortId,
                        //    ticket.ShortId,
                        //    ticket.AssigneeName,
                        //    ticket.Estimation,
                        //    ticket.Link,
                        //    ticket.CreateTime).Result;
                        //if (res.State == CreateTicketState.Ok)
                        {
#if DEBUG
                            Console.WriteLine("Sending TicketInPlayCommand to slack endpoint...");
#endif
                            //TODO: add actualy logic. Just a stab here
                            _bus.Send(new Address(C.SlackEndpoint, C.MachineName), new TicketInPlayCommand
                            {
                                TicketShortId = ticket.ShortId,
                                TaskSummary = ticket.Summary,
                                AssigneeName = ticket.AssigneeName,
                                Estimation = ticket.Estimation,
                                Points = ticket.Estimation * 2/60,
                                LinkToTask = ticket.Link
                            });
                        }
                        continue;
                    }

                    //check assignee
                    if (ticket.AssigneeName == null)
                    {
                        //TODO a.semenov: Remove ticket
                        continue;
                    }

                    _ticketService.UpdateTicket(IntegrationType.Youtrack,
                        ticket.ShortId,
                        ticket.AssigneeName,
                        ticket.Estimation,
                        ticket.State,
                        ticket.Link,
                        ticket.UpdatedTime,
                        null,
                        null);
                    //^ TODO: are we really need to pass here Start/End date?

                    //TODO: temp functionality here:
                    if (ticket.State == TicketState.InProgress && tryTicket.State != ticket.State)
                    {
#if DEBUG
                        Console.WriteLine("Sending TicketBetsClosed command to slack...");
#endif
                        _bus.Send(new Address(C.SlackEndpoint, C.MachineName), new TicketBetsClosed
                        {
                            TicketShortId = ticket.ShortId,
                            TaskSummary = ticket.Summary,
                            AssigneeName = ticket.AssigneeName,
                            Estimation = ticket.Estimation,
                            Delay = TimeSpan.FromMinutes(ticket.Estimation*0.1d),
                            LinkToTask = ticket.Link
                        });
                    }
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine($"Ooops.. Core is done: {e.Message}");
#endif
            }
        }
    }
}
