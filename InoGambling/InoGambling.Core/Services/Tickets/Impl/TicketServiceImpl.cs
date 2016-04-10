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

        public DateTime GetSyncTime()
        {
            DateTime dateTime = DateTime.Now.AddDays(-2);
            try
            {
                dateTime = _ticketRepo.Query().Max(x => x.LastUpdateDate);
            }
            catch (Exception)
            {
            }
            return dateTime;
        }

        public Ticket GetTicket(
            Int64 id)
        {
            return _ticketRepo.GetById(id);
        }

        public Ticket GetTicket(
            IntegrationType integrationType,
            String ticketShortId)
        {
            return
                    _ticketRepo.Query()
                        .FirstOrDefault(x => x.ShortId == ticketShortId && x.IntegrationType == integrationType);
        }

        public CreateTicketResult CreateTicket(
            IntegrationType integrationType,
            String projectShortId,
            String ticketShortId,
            String userName,
            Double estimate,
            String link,
            DateTime createDate,
            Double points)
        {
            try
            {
                var project = _projectService.GetProject(integrationType, projectShortId);
                if (project == null)
                    return new CreateTicketResult()
                    {
                        State = CreateTicketState.ProjectNotExists
                    };
                var ticket = GetTicket(integrationType, ticketShortId);
                if (ticket != null)
                    return new CreateTicketResult()
                    {
                        State = CreateTicketState.TicketExists
                    };
                var user = _userService.GetUser(integrationType, userName);
                if (user == null)
                {
                    var integrationUserRes =
                        _userService.CreateIntegrationUser(null, userName, userName, integrationType, false);
                    user = _userService.GetUser(integrationUserRes.IntegrationUser.UserId);
                }
                ticket = _ticketRepo.Create();
                            
                ticket.State = TicketState.Created;
                ticket.Estimate = estimate;
                ticket.Link = link;
                ticket.IntegrationType = integrationType;
                ticket.ProjectId = project.Id;
                ticket.AssigneeUserId = user.Id;
                ticket.ShortId = ticketShortId;
                ticket.LastUpdateDate = createDate;
                ticket.Points = points;

                ticket = _ticketRepo.Add(ticket);

                _uow.Commit();
                return new CreateTicketResult()
                {
                    State = CreateTicketState.Ok,
                    Ticket = ticket
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
    
        public UpdateTicketResult UpdateTicket(
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
                var ticket = GetTicket(integrationType, ticketShortId);
                if (ticket != null)
                {
                    var user = _userService.GetUser(integrationType, userName);
                    if (user != null)
                    {
                        List<Bet> invalidatedBets = new List<Bet>();
                        //if user changed we invalidate all previous bets
                        if (ticket.AssigneeUserId != user.Id)
                        {
                            //invalidatedBets.AddRange(InvalidateBetsForTicket(ticket.Id));
                        }

                        switch (state)
                        {
                            case TicketState.Canceled:
                            case TicketState.None:
                            case TicketState.Created:
                            case TicketState.ToDo:
                                //invalidatedBets.AddRange(InvalidateBetsForTicket(ticket.Id));
                                break;
                            case TicketState.InProgress:
                                break;
                            case TicketState.OnHold:
                            case TicketState.Verified:
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
                        ticket.LastUpdateDate = lastChangeDate;

                        _ticketRepo.Update(ticket);

                        _uow.Commit();

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

        public IEnumerable<Bet> InvalidateBetsForTicket(Int64 ticketId)
        {
            var bets = _betRepo.Query().Where(x => x.TicketId == ticketId && x.IsInvalidate).ToArray();
            bets.ForEach(x => x.IsInvalidate = true);
            //entities alredy in the db context, so there is no need to push them into repo.Update() method;
            return bets;

        }

        public Boolean IsTicketAllBetsAreOff(Int64 ticketId)
        {
            return IsTicketAllBetsAreOff(GetTicket(ticketId));
        }

        public Boolean IsTicketAllBetsAreOff(IntegrationType integrationType, String ticketShortId)
        {
            return IsTicketAllBetsAreOff(GetTicket(integrationType, ticketShortId)); 
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
                        var allBetsAreOffDeltaMinutes = (Int32)(ticket.Estimate * Constants.ALL_BETS_ARE_OFF_DELTA_PERCENT / 100);
                        if (now - ticket.StartDate.Value > new TimeSpan(0, 0, allBetsAreOffDeltaMinutes, 0))
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
