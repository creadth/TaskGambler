using System;
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
            throw new NotImplementedException();
        }

        public async Task<UpdateIntegrationUserResult> UpdateIntegrationUser(
            Int64 userId,
            String integrationUserName,
            IntegrationType type,
            Boolean isForbidden)
        {
            throw new NotImplementedException();
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
