﻿using System;
using System.Threading.Tasks;
using InoGambling.Core.Services.Users.Models;
using InoGambling.Data.Model;

namespace InoGambling.Core.Services.Users
{
    public interface IUserService
    {
        Task<UserCreateResult> CreateUser(String login, String password);
        Task<User> GetUser(Int64 id, Boolean includeIntegrationUsers = false);
        Task<User> GetUser(String login, Boolean includeIntegrationUsers = false);
        Task<User> GetUser(IntegrationType integrationType, String name);
        Task<UserLoginResult> UserLogin(String login, String password);

        Task<CreateIntegrationUserResult> CreateIntegrationUser(
            Int64 userId, 
            String integrationUserName,
            IntegrationType type, 
            Boolean isForbidden);

        Task<UpdateIntegrationUserResult> UpdateIntegrationUser(
            Int64 userId,
            String integrationUserName,
            IntegrationType type,
            Boolean isForbidden);
    }
}
