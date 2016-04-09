using InoGambling.Data.Models;

namespace InoGambling.Core.Services.Users.Models
{
    public class UpdateIntegrationUserResult
    {
    }

    public enum UpdateIntegrationUserState : byte
    {
        Ok = 0,
        IntegrationUserNotExists = 1,
        UserNotExists = 2,
    }

}
