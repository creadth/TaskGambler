using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using InoGambling.Core.Services.Bets;
using InoGambling.Core.Services.Projects;
using InoGambling.Core.Services.Tickets.Models;
using InoGambling.Core.Services.Users;
using InoGambling.Data;
using InoGambling.Data.Model;
using InoGambling.Data.Repositories;
using Microsoft.Practices.ObjectBuilder2;

namespace InoGambling.Core.Services.Tickets.Impl
{
    public class TicketServiceImpl : ITicketService
    {
        

        public TicketServiceImpl(ITicketRepository ticketRepo, IUserService userService, IBetRepository betRepo, IProjectService projectService, IUnitOfWork uow)
        {
            _ticketRepo = ticketRepo;
            _projectService = projectService;
            _userService = userService;
            _betRepo = betRepo;
            _uow = uow;
        }

        public async Task<Ticket> GetTicket(
            Int64 id)
        {
            return await _ticketRepo.GetById(id);
        }

        public async Task<Ticket> GetTicket(
            IntegrationType integrationType,
            String ticketShortId)
        {
            return
                await
                    _ticketRepo.Query()
                        .FirstOrDefaultAsync(x => x.ShortId == ticketShortId && x.IntegrationType == integrationType);
        }

        public async Task<CreateTicketResult> CreateTicket(
            IntegrationType integrationType,
            String projectShortId,
            String ticketShortId,
            String userName,
            Double estimate,
            String link,
            DateTime createDate)
        {
            try
            {
                var project = _projectService.GetProject(integrationType, projectShortId);
                if (project != null)
                {
                    var ticket = await GetTicket(integrationType, ticketShortId);
                    if (ticket == null)
                    {
                        var user = _userService.GetUser(integrationType, userName);
                        if (user != null)
                        {

                            ticket = _ticketRepo.Create();
                            
                            ticket.State = TicketState.Created;
                            ticket.Estimate = estimate;
                            ticket.Link = link;
                            ticket.IntegrationType = integrationType;
                            ticket.ProjectId = project.Id;
                            ticket.AssigneeUserId = user.Id;

                            ticket = _ticketRepo.Add(ticket);

                            await _uow.CommitAsync();
                            return new CreateTicketResult()
                            {
                                State = CreateTicketState.Ok,
                                Ticket = ticket
                            };
                        }
                        return new CreateTicketResult()
                        {
                            State = CreateTicketState.UserNotExists
                        };
                    }
                    return new CreateTicketResult()
                    {
                        State = CreateTicketState.TicketExists
                    };
                }
                return new CreateTicketResult()
                {
                    State = CreateTicketState.ProjectNotExists
                };
            }
            catch (Exception e)
            {
                return new CreateTicketResult()
                {
                    State = CreateTicketState.Error
                };
            }
        }

        public async Task<UpdateTicketResult> UpdateTicket(
            IntegrationType integrationType,
            String ticketShortId,
            String userName,
            Double estimate,
            TicketState state,
            String link,
            DateTime lastChangeDate,
            DateTime? startDate,
            DateTime? endDate)
        {
            try
            {
                var ticket = await GetTicket(integrationType, ticketShortId);
                if (ticket != null)
                {
                    var user = _userService.GetUser(integrationType, userName);
                    if (user != null)
                    {
                        List<Bet> invalidatedBets = new List<Bet>();
                        //if user changed we invalidate all previous bets
                        if (ticket.AssigneeUserId != user.Id)
                        {
                            invalidatedBets.AddRange(await InvalidateBetsForTicket(ticket.Id));
                        }

                        switch (state)
                        {
                            case TicketState.Canceled:
                            case TicketState.None:
                            case TicketState.Created:
                            case TicketState.ToDo:
                                invalidatedBets.AddRange(await InvalidateBetsForTicket(ticket.Id));
                                break;
                            case TicketState.InProgress:
                                if (ticket.State != TicketState.OnHold)
                                {
                                    invalidatedBets.AddRange(await InvalidateBetsForTicket(ticket.Id));
                                }
                                break;
                            case TicketState.OnHold:
                                if (ticket.State == TicketState.InProgress)
                                {
                                    ticket.ExecutionTime += lastChangeDate - ticket.LastUpdateDate; //not exactly correct, but for now it's best solution
                                }
                                break;
                        }
                        ticket.State = state;
                        ticket.AssigneeUserId = user.Id;
                        ticket.Estimate = estimate;
                        ticket.StartDate = startDate;
                        ticket.EndDate = endDate;
                        ticket.Link = link;

                        _ticketRepo.Update(ticket);

                        await _uow.CommitAsync();

                        return new UpdateTicketResult()
                        {
                            Ticket = ticket,
                            InvalidatedBets = invalidatedBets.Distinct(),
                            State = UpdateTicketState.Ok
                        };

                    }
                    return new UpdateTicketResult()
                    {
                        State = UpdateTicketState.UserNotExists
                    };
                }
                return new UpdateTicketResult()
                {
                    State = UpdateTicketState.TicketNotExists
                };
            }
            catch (Exception e)
            {
                return new UpdateTicketResult()
                {
                    State = UpdateTicketState.Error
                };
            }
        }

        public async Task<IEnumerable<Bet>> InvalidateBetsForTicket(Int64 ticketId)
        {
            var bets = await _betRepo.Query().Where(x => x.TicketId == ticketId && x.IsInvalidate).ToArrayAsync();
            bets.ForEach(x => x.IsInvalidate = true);
            //entities alredy in the db context, so there is no need to push them into repo.Update() method;
            return bets;

        }

        public async Task<Boolean> IsTicketAllBetsAreOff(Int64 ticketId)
        {
            return IsTicketAllBetsAreOff(await GetTicket(ticketId));
        }

        public async Task<Boolean> IsTicketAllBetsAreOff(IntegrationType integrationType, String ticketShortId)
        {
            return IsTicketAllBetsAreOff(await GetTicket(integrationType, ticketShortId)); 
        }

    public Boolean IsTicketAllBetsAreOff(Ticket ticket)
        {
            var now = DateTime.Now;
            switch (ticket.State)
            {
                case TicketState.Created:
                case TicketState.ToDo:
                    return false;
                case TicketState.InProgress:
                    if (ticket.StartDate.HasValue)
                    {
                        if (ticket.StartDate.Value < now)
                        {
                            return false;
                        }
                        //bullshit
                        var estimationHours = ticket.Estimate * Constants.ESTIMATE_COEF;
                        var allBetsAreOffDeltaSeconds = (Int32)(estimationHours * Constants.ALL_BETS_ARE_OFF_DELTA_PERCENT * 3600);
                        if (now - ticket.StartDate.Value > new TimeSpan(0, 0, 0, allBetsAreOffDeltaSeconds))
                        {
                            return true;
                        }
                    }
                    return false;
                default:
                    return true;
            }
        }

        private readonly ITicketRepository _ticketRepo;
        private readonly IProjectService _projectService;
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IBetRepository _betRepo;
    }
}
