using System;
using System.Threading.Tasks;
using InoGambling.Core.Services.Users.Models;
using InoGambling.Data.Models;

namespace InoGambling.Core.Services.Users
{
    public interface IUserService
    {
        Task<UserCreateResult> CreateUser(String login, String password);
        Task<User> GetUser(Int64 id, Boolean includeIntegrationUsers);
        Task<User> GetUser(String login, Boolean includeIntegrationUsers);
        Task<UserLoginResult> UserLogin(String login, String password);

        Task<CreateIntegrationUserResult> CreateIntegrationUser(
            Int64 userId, 
            String integrationUserName,
            IntegrationType type);
    }
}
