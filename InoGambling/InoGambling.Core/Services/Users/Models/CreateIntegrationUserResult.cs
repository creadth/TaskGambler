using InoGambling.Data.Model;

namespace InoGambling.Core.Services.Users.Models
{
    public class CreateIntegrationUserResult
    {
        public CreateIntegrationUserState State { get; set; }
        public IntegrationUser IntegrationUser { get; set; }
    }

    public enum CreateIntegrationUserState : byte
    {
        Ok = 0,
        UserNotExists = 1,
        IntegrationUserExists = 2,

        Error = 100,
    }
}
