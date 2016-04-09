using System;
using System.Data.Entity;
using System.Threading.Tasks;
using InoGambling.Core.Services.Projects;
using InoGambling.Core.Services.Tickets.Models;
using InoGambling.Core.Services.Users;
using InoGambling.Data;
using InoGambling.Data.Model;
using InoGambling.Data.Repositories;

namespace InoGambling.Core.Services.Tickets.Impl
{
    public class TicketServiceImpl : ITicketService
    {
        public TicketServiceImpl(ITicketRepository ticketRepo, IUserService userService, IProjectService projectService, IUnitOfWork uow)
        {
            _ticketRepo = ticketRepo;
            _projectService = projectService;
            _userService = userService;
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

        private readonly ITicketRepository _ticketRepo;
        private readonly IProjectService _projectService;
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
    }
}
