using InoGambling.Data.Model;

namespace InoGambling.Core.Services.Users.Models
{
    public class UpdateIntegrationUserResult
    {
        public UpdateIntegrationUserState State { get; set; }
        public IntegrationUser IntegrationUser { get; set; }
    }

    public enum UpdateIntegrationUserState : byte
    {
        Ok = 0,
        IntegrationUserNotExists = 1,
        UserNotExists = 2,

        Error = 100,
    }

}
