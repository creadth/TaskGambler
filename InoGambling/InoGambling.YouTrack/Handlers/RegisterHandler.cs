using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InoGambling.CommonMessages.Commands.Integrations.Slack;
using InoGambling.CommonMessages.Commands.Integrations.YouTrack;
using InoGambling.Framework.Intergations.Trackers;
using NServiceBus;
using C = InoGambling.Framework.BeautifulConstants;
namespace InoGambling.YouTrack.Handlers
{
    public class RegisterHandler
        : IHandleMessages<YoutrackDistributedRegisterCommand>
    {

        private Address _coreAddress;
        private Address _slackAddress;

        private IBus _bus;
        private IYouTrackIntegration _youTrackIntegration;

        public RegisterHandler(IBus bus, IYouTrackIntegration youTrackIntegration)
        {
            _bus = bus;
            _youTrackIntegration = youTrackIntegration;
            _coreAddress = new Address(C.CoreEndpoint, C.MachineName);
            _slackAddress = new Address(C.SlackEndpoint,C.MachineName);
        }

        public void Handle(YoutrackDistributedRegisterCommand message)
        {
            var displayName = _youTrackIntegration.ValidateUserLogin(message.YouTrackLogin);
            if (displayName == null)
            {
                _bus.Send(_slackAddress, new RegisterResult
                {
                    UserId = message.LegacyCommand.UserId,
                    RegOk = false,
                    AdditionalMessage = $" someone has just stolen you login from YouTrack. Was it evil cucumber? We will never be sure... :("
                });
                //no need to notify core about accident with wrong youtrack credential. Let core rest.
            }
            else
            {
                //post back to core update reg message. Core be good.
                message.LegacyCommand.YoutrackConfirmed = true;
                message.LegacyCommand.YouTrackDisplayName = displayName;
                _bus.Send(_coreAddress, message.LegacyCommand);
            }
        }
    }
}
