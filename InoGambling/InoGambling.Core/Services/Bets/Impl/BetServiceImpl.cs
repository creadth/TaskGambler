﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using InoGambling.Core.Services.Bets.Models;
using InoGambling.Core.Services.Tickets;
using InoGambling.Core.Services.Users;
using InoGambling.Data;
using InoGambling.Data.Model;
using InoGambling.Data.Repositories;

namespace InoGambling.Core.Services.Bets.Impl
{
    public class BetServiceImpl : IBetService
    {
        public BetServiceImpl(IBetRepository betRepo, ITicketService ticketService, IUserService userService, IUnitOfWork uow)
        {
            _betRepo = betRepo;
            _ticketService = ticketService;
            _userService = userService;
            _uow = uow;
        }

        public async Task<MakeBetResult> MakeBet(
            Int64 userId,
            Int64 ticketId,
            Double estimate,
            Double points,
            Boolean isAgree)

        {
            try
            {
                //not really cool logic, but for now it's ok
                var user = await _userService.GetUser(userId, true);
                if (user == null)
                {
                    return new MakeBetResult()
                    {
                        State = MakeBetState.UserNotExists
                    };
                }

                var ticket = await _ticketService.GetTicket(ticketId);
                if (ticket == null)
                {
                    return new MakeBetResult()
                    {
                        State = MakeBetState.TicketNotExists
                    };
                }

                if (_ticketService.IsTicketAllBetsAreOff(ticket))
                {
                    return new MakeBetResult()
                    {
                        State = MakeBetState.AllBetsAreOff
                    };
                }

                var bet = await _betRepo.Query()
                    .FirstOrDefaultAsync(x => x.TicketId == ticket.Id && x.UserId == user.Id && !x.IsInvalidate && !x.IsCanceled);
                if (bet != null)
                {
                    var cancelResult = await CancelBet(bet.Id);
                    if (cancelResult.State != CancelBetState.Ok)
                    {
                        return new MakeBetResult()
                        {
                            State = MakeBetState.Error
                        };
                    }
                }
                
                //TODO:check points balance;
                //TODO: check forbidden user;
                //TODO: check forbidden project;

                bet = _betRepo.Create();

                bet.UserId = user.Id;
                bet.TicketId = ticket.Id;
                bet.Estimate = estimate;
                bet.IsAgree = isAgree;

                bet.Points = points;
                user.Points -= points;

                bet = _betRepo.Add(bet);
                await _uow.CommitAsync();
                return new MakeBetResult()
                {
                    Bet = bet,
                    State = MakeBetState.Ok
                };

            }
            catch (Exception e)
            {
                return new MakeBetResult()
                {
                    State = MakeBetState.Error
                };
            }
        }

        public async Task<MakeBetResult> MakeBet(
            IntegrationType integrationType,
            String userName,
            String ticketShortId,
            Double estimate,
            Double points,
            Boolean isAgree)
        {
            try
            {
                //not really cool logic, but for now it's ok
                var user = await _userService.GetUser(integrationType, userName);
                if (user == null)
                {
                    return new MakeBetResult()
                    {
                        State = MakeBetState.UserNotExists
                    };
                }

                var ticket = await _ticketService.GetTicket(integrationType, ticketShortId);
                if (ticket == null)
                {
                    return new MakeBetResult()
                    {
                        State = MakeBetState.TicketNotExists
                    };
                }

                if (_ticketService.IsTicketAllBetsAreOff(ticket))
                {
                    return new MakeBetResult()
                    {
                        State = MakeBetState.AllBetsAreOff
                    };
                }

                var bet = await _betRepo.Query()
                    .FirstOrDefaultAsync(x => x.TicketId == ticket.Id && x.UserId == user.Id && !x.IsInvalidate && !x.IsCanceled);
                if (bet != null)
                {
                    var cancelResult = await CancelBet(bet.Id);
                    if (cancelResult.State != CancelBetState.Ok)
                    {
                        return new MakeBetResult()
                        {
                            State = MakeBetState.Error
                        };
                    }
                }

                
                //TODO:check points balance;
                //TODO: check forbidden user;
                //TODO: check forbidden project;

                bet = _betRepo.Create();

                bet.UserId = user.Id;
                bet.TicketId = ticket.Id;
                bet.Estimate = estimate;
                bet.IsAgree = isAgree;
                
                bet.Points = points;
                user.Points -= points;

                bet = _betRepo.Add(bet);
                await _uow.CommitAsync();
                return new MakeBetResult()
                {
                    Bet = bet,
                    State = MakeBetState.Ok
                };

            }
            catch (Exception e)
            {
                return new MakeBetResult()
                {
                    State = MakeBetState.Error
                };
            }
        }

        public async Task<CancelBetResult> CancelBet(
            Int64 betId)
        {
            try
            {
                var bet = await _betRepo.Query()
                    .FirstOrDefaultAsync(x => x.Id == betId && !x.IsInvalidate && !x.IsCanceled);
                if (bet == null)
                {
                    return new CancelBetResult()
                    {
                        State = CancelBetState.BetNotExists
                    };
                }
                bet.IsCanceled = true;
                bet = _betRepo.Update(bet);

                await _uow.CommitAsync();

                return new CancelBetResult()
                {
                    State = CancelBetState.Ok
                };
            }
            catch (Exception e)
            {
                return new CancelBetResult()
                {
                    State = CancelBetState.Error
                };
            }
        }

        public async Task<CancelBetResult> CancelBet(
            IntegrationType integrationType,
            String userName,
            String ticketShortId)
        {
            try
            {
                //not really cool logic, but for now it's ok
                var user = await _userService.GetUser(integrationType, userName);
                if (user == null)
                {
                    return new CancelBetResult()
                    {
                        State = CancelBetState.UserNotExists
                    };
                }

                var ticket = await _ticketService.GetTicket(integrationType, ticketShortId);
                if (ticket == null)
                {
                    return new CancelBetResult()
                    {
                        State = CancelBetState.TicketNotExists
                    };
                }

                var bet = await _betRepo.Query()
                    .FirstOrDefaultAsync(x => x.TicketId == ticket.Id && x.UserId == user.Id && !x.IsInvalidate && !x.IsCanceled);
                if (bet == null)
                {
                    return new CancelBetResult()
                    {
                        State = CancelBetState.BetNotExists
                    };
                }
                bet.IsCanceled = true;
                bet = _betRepo.Update(bet);
                await _uow.CommitAsync();
                return new CancelBetResult()
                {
                    State = CancelBetState.Ok
                };

            }
            catch (Exception e)
            {
                return new CancelBetResult()
                {
                    State = CancelBetState.Error
                };
            }
        }

        public async Task<PlayTicketResult> PlayTicket(Int64 ticketId)
        {
            var result = new PlayTicketResult();

            var ticket = await _ticketService.GetTicket(ticketId);
            result.Ticket = ticket;

            var execDelta = Math.Abs(ticket.Estimate - ticket.ExecutionTime.TotalMinutes);
            var isWon = execDelta <= ticket.Estimate * Constants.WIN_ESTIMATE_DELTA / 100;
            var bets = await _betRepo.Query().Where(x => x.TicketId == ticketId).ToArrayAsync();

            var pointsForAssignee = isWon
                ? 2*ticket.Points + (ticket.Points*Constants.RAKE_PERCENT/100*bets.Count(x => !x.IsAgree)) : 0;
            var pointsForAssigneeDisplay = isWon ? pointsForAssignee - ticket.Points : 0;

            var pointsForWinners = isWon
                ? 2 * ticket.Points : 0;
            var pointsForWinnersDisplay = isWon ? ticket.Points : -ticket.Points;
            var pointsForLoosersDisplay = isWon ? -ticket.Points : ticket.Points;

            var asssigneeUser = await _userService.GetUser(ticket.AssigneeUserId);
            asssigneeUser.Points += pointsForAssignee;
            result.AssigneeReesult = new PlayTicketUserResult()
            {
                PointsResult = pointsForAssigneeDisplay,
                User = asssigneeUser,
                Win = isWon
            };

            var userResults = new List<PlayTicketUserResult>();
            foreach (var bet in bets)
            {
                var user = await _userService.GetUser(bet.UserId);
                var userWon = bet.IsAgree == isWon;
                if (userWon)
                {
                    user.Points += pointsForWinners;
                }
                userResults.Add(new PlayTicketUserResult()
                {
                    User =  user,
                    Win = userWon,
                    PointsResult = userWon ? pointsForAssigneeDisplay : pointsForLoosersDisplay
                });

            }
            result.PlayersResults = userResults.ToArray();
            await _uow.CommitAsync();
            return result;
        }



        private readonly IBetRepository _betRepo;
        private readonly IUserService _userService;
        private readonly ITicketService _ticketService;
        private readonly IUnitOfWork _uow;
    }
}
