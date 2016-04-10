using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InoGambling.CommonMessages.Commands.Integrations.Slack;
using InoGambling.CommonMessages.Commands.Integrations.YouTrack;
using InoGambling.Core.Services.Bets;
using InoGambling.Core.Services.Bets.Models;
using InoGambling.Core.Services.Tickets;
using InoGambling.Core.Services.Users;
using InoGambling.Core.Services.Users.Models;
using InoGambling.Data.Model;
using NServiceBus;
using C = InoGambling.Framework.BeautifulConstants;
namespace InoGambling.Core.Handlers
{
    public class SlackHandler
        : IHandleMessages<RegisterCommand>,
          IHandleMessages<BetCommand>,
          IHandleMessages<StatsCommand>

    {
        private IBus _bus;
        private IUserService _userService;
        private ITicketService _ticketService;
        private IBetService _betService;

        private Address _slackAddress;
        private Address _youtrackAddress;


        public SlackHandler(IBus bus, IUserService userService, ITicketService ticketService, IBetService betService)
        {
            _bus = bus;
            _userService = userService;
            _betService = betService;
            _slackAddress = new Address(C.SlackEndpoint, C.MachineName);
            _youtrackAddress = new Address(C.YouTrackEndpoint, C.MachineName);
            _ticketService = ticketService;
        }

        public void Handle(RegisterCommand message)
        {
            var trySlackUser = _userService.GetIntegrationUser(IntegrationType.Slack, message.UserId);
            if (trySlackUser != null)
            {
                _bus.Send(_slackAddress, new RegisterResult
                {
                    UserId = message.UserId,
                    RegOk = false,
                    AdditionalMessage = "you have already registered. Don't blame evil cucumber."
                });
                return;
            }
            var tryYouTrackUser =
                _userService.GetIntegrationUser(IntegrationType.Youtrack, message.YouTrackLogin);
            if (tryYouTrackUser == null)
            {
                //Hard case. We need to retrieve youtrack user 
                //from youtrack integration and continue registration process                

                //YoutrackConfirmed flag represents that youtrack login
                //was already checked and confirmed as existed
                if (message.YoutrackConfirmed)
                {
                    var youtrackRes = _userService.CreateIntegrationUser(
                        null,
                        message.YouTrackLogin,
                        message.YouTrackDisplayName,
                        IntegrationType.Youtrack,
                        false
                        );
                    if (youtrackRes.State != CreateIntegrationUserState.Ok)
                    {
                        SendUnknownErrorMessageForRegister(message.UserId);
                        return;
                    }
                    tryYouTrackUser = youtrackRes.IntegrationUser;
                }
                else
                {
                    _bus.Send(_youtrackAddress, new YoutrackDistributedRegisterCommand
                    {
                        LegacyCommand = message,
                        YouTrackLogin = message.YouTrackLogin,
                        RegIntegrationUserId = message.UserId
                    });
                    return;
                }

            }
            var res = _userService.CreateIntegrationUser(tryYouTrackUser.UserId, message.UserId, message.UserName,
                IntegrationType.Slack, false);
            if (res.State != CreateIntegrationUserState.Ok)
            {
                SendUnknownErrorMessageForRegister(message.UserId);
                return;
            }
            _userService.UpdateUserPoints(tryYouTrackUser.UserId, C.StartingPoints);
            _bus.Send(_slackAddress, new RegisterResult
            {
                UserId = message.UserId,
                AdditionalMessage = string.Empty,
                RegOk = true
            });
        }

        protected void SendUnknownErrorMessageForRegister(string userId)
        {
            _bus.Send(_slackAddress,
                new RegisterResult
                {
                    UserId = userId,
                    RegOk = false,
                    AdditionalMessage = " we don't know what happened. It just failed. Probably, it was evil cucumber. Circumstances are unclear..."
                });
        }

        public void Handle(BetCommand message)
        {
            var tryUser = _userService.GetIntegrationUser(IntegrationType.Slack, message.UserId);
            if (tryUser == null)
            {
                _bus.Send(_slackAddress, new BetResponse
                {
                    IsOk = false,
                    UserId = message.UserId,
                    AdditionalMessage = " I just don't know you, who are you, what are you? Come closer, say _reg_ to my face ;)"
                });
                return;
            }
            var tryTicket = _ticketService.GetTicket(IntegrationType.Youtrack, message.TaskShortId);
            if (tryTicket == null)
            {
                _bus.Send(_slackAddress, new BetResponse
                {
                    UserId = message.UserId,
                    AdditionalMessage = "evil cucumber has lied you about ticket number. Avoid cucumber next time.",
                    IsOk = false
                });
            }
            else
            {
                if (_ticketService.IsTicketAllBetsAreOff(IntegrationType.Youtrack, message.TaskShortId))
                {
                    _bus.Send(_slackAddress, new BetResponse
                    {
                        IsOk = false,
                        UserId = message.UserId,
                        AdditionalMessage = "evil cucumber just made last bet on this task. Sorry, mister slowpoke (but we still love you despite your speed problems)"
                    });
                    return;
                }
                if (tryTicket.AssigneeUserId == tryUser.UserId)
                {
                    _bus.Send(_slackAddress, new BetResponse
                    {
                        IsOk = false,
                        UserId = message.UserId,
                        AdditionalMessage = " Even evil cucumber never did so. Trying to bet your own tickets, really? Nice try."
                    });
                    return;                    
                }
                var normalUser = _userService.GetUser(tryUser.UserId);
                if (normalUser.Points <= tryTicket.Points)
                {
                    _bus.Send(_slackAddress, new BetResponse
                    {
                        IsOk = false,
                        UserId = message.UserId,
                        AdditionalMessage = "seems evil cucumber took your points. Bad, bad cucumber."
                    });
                    return;
                }
                var makeBetResult = _betService
                    .MakeBet(normalUser.Id, tryTicket.Id, tryTicket.Estimate, tryTicket.Points, !message.IsBetAgainst);
                if (makeBetResult.State == MakeBetState.Ok)
                {
                    var np = normalUser.Points - tryTicket.Points;
                    _userService.UpdateUserPoints(normalUser.Id, np);
                    _bus.Send(_slackAddress, new BetResponse
                    {
                        IsOk = true,
                        UserId = message.UserId,
                        AdditionalMessage = $" your balance now is {np} Points. Good estimating!"
                    });
                }
                else
                {
                    _bus.Send(_slackAddress, new BetResponse
                    {
                        IsOk = false,
                        UserId = message.UserId,
                        AdditionalMessage = "evil cucumber just casted evil cucumber spell on you. Roll d20. Your ~bat~ bet died.. Bad, bad cucumber."
                    });
                }
                
            }
            

        }

        public void Handle(StatsCommand message)
        {
            var tryUser = _userService.GetIntegrationUser(IntegrationType.Slack, message.UserId);
            if (tryUser == null)
            {
                message.IsOk = false;
                message.AdditionalMessage = " I don't know who you are. Say me _reg_ I'll give you points ;)";
                _bus.Send(_slackAddress, message);
                return;
            }
            var normalUser = _userService.GetUser(tryUser.UserId);
            message.IsOk = true;
            message.Points = normalUser.Points;
            _bus.Send(_slackAddress, message);

        }
    }
}
