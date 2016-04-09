﻿using System;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;
using InoGambling.Core.Services.Users.Models;
using InoGambling.Data;
using InoGambling.Data.Model;
using InoGambling.Data.Repositories;

namespace InoGambling.Core.Services.Users.Impl
{
    public class UserServiceImpl : IUserService
    {
        public UserServiceImpl(
            IUserRepository userRepo, 
            IIntegrationUserRepository integrationUserRepo, 
            IUnitOfWork uow)
        {
            _userRepo = userRepo;
            _integrationUserRepo = integrationUserRepo;
            _uow = uow;
        }

        public async Task<UserCreateResult> CreateUser(String login, String password)
        {
            try
            {
                var user = await GetUser(login);
                if (user == null)
                {
                    user = _userRepo.Create();
                    user.Login = login;
                    user.Password = GetPasswordHash(password);
                    user = _userRepo.Add(user);
                    await _uow.CommitAsync();

                    return new UserCreateResult()
                    {
                        State = UserCreateState.Ok,
                        UserId = user.Id
                    };
                }
                else
                {
                    return new UserCreateResult()
                    {
                        State = UserCreateState.LoginExists
                    };
                }
            }
            catch (Exception e)
            {
                return new UserCreateResult()
                {
                    State = UserCreateState.Error
                };
            }
        }

        public async Task<User> GetUser(Int64 id, Boolean includeIntegrationUsers = false)
        {
            var query = _userRepo.Query();
            if (includeIntegrationUsers)
            {
                query = query.Include("IntegrationUsers");
            }
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetUser(String login, Boolean includeIntegrationUsers = false)
        {
            var query = _userRepo.Query();
            if (includeIntegrationUsers)
            {
                query = query.Include("IntegrationUsers");
            }
            return await query.FirstOrDefaultAsync(x => x.Login == login);
        }

        public async Task<User> GetUser(IntegrationType integrationType, String name)
        {
            return await 
                _userRepo.Query()
                    .Include("IntegrationUsers")
                    .FirstOrDefaultAsync(x => x.IntegrationUsers.Any(y => y.Type == integrationType && y.Name == name));
        }

        public async Task<UserLoginResult> UserLogin(String login, String password)
        {
            try
            {
                var user = await GetUser(login);
                if (user != null)
                {
                    if (user.Password == GetPasswordHash(password))
                    {
                        return new UserLoginResult()
                        {
                            State = UserLoginState.Ok,
                            UserId = user.Id
                        };
                    }
                    else
                    {
                        return new UserLoginResult()
                        {
                            State = UserLoginState.IncorrectPassword
                        };
                    }
                }
                else
                {
                    return new UserLoginResult()
                    {
                        State = UserLoginState.LoginNotExists,
                    };
                }
                
            }
            catch (Exception)
            {
                return new UserLoginResult()
                {
                    State = UserLoginState.Error
                };
            }
        }

        public async Task<CreateIntegrationUserResult> CreateIntegrationUser(
            Int64 userId,
            String integrationUserName,
            IntegrationType type,
            Boolean isForbidden)
        {
            try
            {
                var user = await GetUser(userId, true);
                if (user != null)
                {
                    if (user.IntegrationUsers.All(x => x.Type != type))
                    {
                        var integrationUser = _integrationUserRepo.Create();
                        integrationUser.Name = integrationUserName;
                        integrationUser.Type = type;
                        integrationUser.UserId = user.Id;
                        integrationUser.IsForbidden = true;
                        integrationUser = _integrationUserRepo.Add(integrationUser);
                        await _uow.CommitAsync();

                        return new CreateIntegrationUserResult()
                        {
                            State = CreateIntegrationUserState.Ok,
                            IntegrationUser = integrationUser
                        };
                    }
                    return new CreateIntegrationUserResult()
                    {
                        State = CreateIntegrationUserState.IntegrationUserExists
                    };
                }
                return new CreateIntegrationUserResult()
                {
                    State = CreateIntegrationUserState.UserNotExists
                };

            }
            catch (Exception)
            {
                return new CreateIntegrationUserResult()
                {
                    State = CreateIntegrationUserState.Error
                };
            }
        }

        public async Task<UpdateIntegrationUserResult> UpdateIntegrationUser(
            Int64 userId,
            String integrationUserName,
            IntegrationType type,
            Boolean isForbidden)
        {
            try
            {
                var user = await GetUser(userId, true);
                if (user != null)
                {
                    var integrationUser = user.IntegrationUsers.FirstOrDefault(x => x.Type == type);
                    if (integrationUser != null)
                    {
                        integrationUser.Name = integrationUserName;
                        integrationUser.IsForbidden = isForbidden;
                        integrationUser = _integrationUserRepo.Update(integrationUser);
                        await _uow.CommitAsync();

                        return new UpdateIntegrationUserResult()
                        {
                            State = UpdateIntegrationUserState.Ok,
                            IntegrationUser = integrationUser
                        };
                    }
                    return new UpdateIntegrationUserResult()
                    {
                        State = UpdateIntegrationUserState.IntegrationUserNotExists
                    };
                }
                return new UpdateIntegrationUserResult()
                {
                    State = UpdateIntegrationUserState.UserNotExists
                };

            }
            catch (Exception)
            {
                return new UpdateIntegrationUserResult()
                {
                    State = UpdateIntegrationUserState.Error
                };
            }
        }

        private String GetPasswordHash(String password)
        {
            return password;
        }


        private readonly IUserRepository _userRepo;
        private readonly IIntegrationUserRepository _integrationUserRepo;
        private readonly IUnitOfWork _uow;
    }
}