using System;
using System.Threading.Tasks;
using InoGambling.Core.Services.Users.Models;
using InoGambling.Data.Model;

namespace InoGambling.Core.Services.Users
{
    public interface IUserService
    {
        UserCreateResult CreateUser(String login, String password);
        UserCreateResult CreateUser();
        User GetUser(Int64 id, Boolean includeIntegrationUsers = false);
        User GetUser(String login, Boolean includeIntegrationUsers = false);
        User GetUser(IntegrationType integrationType, String name);
        UserLoginResult UserLogin(String login, String password);
        void UpdateUserPoints(Int64 userId, Double points);

        CreateIntegrationUserResult CreateIntegrationUser(
            Int64? userId,
            String integrationUserName,
            String integrationUserDisplayName,
            IntegrationType type,
            Boolean isForbidden);

        UpdateIntegrationUserResult UpdateIntegrationUser(
            Int64 userId,
            String integrationUserName,
            String integrationUserDisplayName,
            IntegrationType type,
            Boolean isForbidden);

        IntegrationUser GetIntegrationUser(
            IntegrationType integrationType,
            String integrationUserName);

        IntegrationUser GetIntegrationUser(Int64 integrationUserId);
    }
}
