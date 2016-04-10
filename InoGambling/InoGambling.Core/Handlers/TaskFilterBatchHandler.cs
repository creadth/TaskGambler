using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using InoGambling.CommonMessages.Commands.Integrations;
using InoGambling.CommonMessages.Commands.Integrations.Slack;
using InoGambling.CommonMessages.Commands.Integrations.YouTrack;
using InoGambling.Core.Services.Bets;
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
        public IBetService _betService;

        public TaskFilterBatchHandler(IBus bus,
            ITicketService ticketService,
            IProjectService projectService,
            IUserService userService,
            IBetService betService)
        {
            _bus = bus;
            _ticketService = ticketService;
            _projectService = projectService;
            _userService = userService;
            _betService = betService;
        }

        public void Handle(TaskFilterBatchCommand message)
        {
#if DEBUG
            Console.WriteLine($"{message.Tickets.Count} Integration {message.Integration} tickets received");
#endif
            try {
                foreach (var ticket in message.Tickets)
                {
                    var tryTicket = _ticketService.GetTicket(IntegrationType.Youtrack, ticket.ShortId);
                    if (tryTicket == null)
                    {
                        if (ticket.AssigneeName == null) continue;
                        if (ticket.Estimation == 0) continue;
                        if (_projectService
                            .CreateProject(IntegrationType.Youtrack, ticket.ProjectShortId)
                            .State == CreateProjectState.Error)
                        {
                            continue;
                        }
                        var user = _userService.GetIntegrationUser(IntegrationType.Youtrack, ticket.AssigneeName);
                        if (user == null)
                        {
                            if (_userService.CreateIntegrationUser(null, ticket.AssigneeName, ticket.AssigneeName,
                                IntegrationType.Youtrack, false).State == CreateIntegrationUserState.Error)
                            {
                                continue;
                            }
                        }

                        var res = _ticketService.CreateTicket(IntegrationType.Youtrack,
                            ticket.ProjectShortId,
                            ticket.ShortId,
                            ticket.AssigneeName,
                            ticket.Estimation,
                            ticket.Link,
                            ticket.CreateTime,
                            Math.Ceiling(2d * ticket.Estimation / 60d)
                            );
                        if (res.State != CreateTicketState.Ok)
                        {
                            //TODO: log error?
                            continue;
                        }
                        _bus.Send(new Address(C.SlackEndpoint, C.MachineName), new TicketInPlayCommand
                        {
                            TicketShortId = ticket.ShortId,
                            TaskSummary = ticket.Summary,
                            AssigneeName = ticket.AssigneeName,
                            Estimation = ticket.Estimation,
                            Points = res.Ticket.Points,
                            LinkToTask = ticket.Link
                        });

                        tryTicket = res.Ticket;
                    }

                    //check assignee
                    if (ticket.AssigneeName == null)
                    {
                        //TODO a.semenov: Remove ticket
                        continue;
                    }

                    var oldState = tryTicket.State;

                    var updateResult = _ticketService.UpdateTicket(IntegrationType.Youtrack,
                        ticket.ShortId,
                        ticket.AssigneeName,
                        ticket.Estimation,
                        ticket.State,
                        ticket.Link,
                        ticket.UpdatedTime,
                        null,
                        null);
                    //^ TODO: are we really need to pass here Start/End date?


                    if (ticket.State == TicketState.Verified && oldState != ticket.State)
                    {
                        var res = _betService.PlayTicket(tryTicket.Id);
                        var results = res.PlayersResults.Select(x => new TicketResult
                        {
                            UserId = x.User.IntegrationUsers.FirstOrDefault(u => u.Type == IntegrationType.Slack)?.Name,
                            AmtChange = x.PointsResult,
                            CurrentPoints = x.User.Points,
                            HasWon = x.Win
                        }).ToList();
                        _bus.Send(new Address(C.SlackEndpoint, C.MachineName), new TicketPlayFinished
                        {
                            TicketId = res.Ticket.ShortId,
                            TicketLink = res.Ticket.Link,
                            Results = results
                        });
                        return;
                    }

                    if (ticket.State == TicketState.InProgress && oldState != ticket.State)
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
                            Delay = TimeSpan.FromMinutes(ticket.Estimation*C.TimeWindowEstimationPercentage),
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
