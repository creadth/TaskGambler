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

        public UserCreateResult CreateUser(String login, String password)
        {
            try
            {
                var user = GetUser(login);
                if (user == null)
                {
                    user = _userRepo.Create();
                    user.Login = login;
                    user.Password = GetPasswordHash(password);
                    user = _userRepo.Add(user);
                    _uow.Commit();

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

        public void UpdateUserPoints(Int64 userId, Double points)
        {
            var user = _userRepo.Query().FirstOrDefault(x => x.Id == userId);
            user.Points = points;
            _userRepo.Update(user);
            _uow.Commit();
        }

        public UserCreateResult CreateUser()
        {
            try
            {
                var user = _userRepo.Create();
                user = _userRepo.Add(user);
                _uow.Commit();

                return new UserCreateResult()
                {
                    State = UserCreateState.Ok,
                    UserId = user.Id
                };
            }
            catch (Exception e)
            {
                return new UserCreateResult()
                {
                    State = UserCreateState.Error
                };
            }
        }

        public User GetUser(Int64 id, Boolean includeIntegrationUsers = false)
        {
            var query = _userRepo.Query();
            if (includeIntegrationUsers)
            {
                query = query.Include("IntegrationUsers");
            }
            return query.FirstOrDefault(x => x.Id == id);
        }

        public User GetUser(String login, Boolean includeIntegrationUsers = false)
        {
            var query = _userRepo.Query();
            if (includeIntegrationUsers)
            {
                query = query.Include("IntegrationUsers");
            }
            return query.FirstOrDefault(x => x.Login == login);
        }

        public User GetUser(IntegrationType integrationType, String name)
        {
            return
                _userRepo.Query()
                    .Include("IntegrationUsers")
                    .FirstOrDefault(x => x.IntegrationUsers.Any(y => y.Type == integrationType && y.Name == name));
        }

        public UserLoginResult UserLogin(String login, String password)
        {
            try
            {
                var user = GetUser(login);
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
            catch (Exception e)
            {
                return new UserLoginResult()
                {
                    State = UserLoginState.Error
                };
            }
        }

        public CreateIntegrationUserResult CreateIntegrationUser(
            Int64? userId,
            String integrationUserName,
            String integrationUserDisplayName,
            IntegrationType type,
            Boolean isForbidden)
        {
            try
            {
                if (!userId.HasValue)
                {
                    var integrationUser = GetIntegrationUser(type, integrationUserName);
                    if (integrationUser != null)
                    {
                        return new CreateIntegrationUserResult()
                        {
                            State = CreateIntegrationUserState.IntegrationUserExists
                        };
                    }

                    var userCreateResult = CreateUser();
                    if (userCreateResult.State != UserCreateState.Ok)
                    {
                        return new CreateIntegrationUserResult()
                        {
                            State = CreateIntegrationUserState.Error
                        };
                    }
                    userId = userCreateResult.UserId;
                }
                var user = GetUser(userId.Value, true);
                if (user != null)
                {
                    if (user.IntegrationUsers.All(x => x.Type != type))
                    {
                        var integrationUser = _integrationUserRepo.Create();
                        integrationUser.Name = integrationUserName;
                        integrationUser.DisplayName = integrationUserDisplayName;
                        integrationUser.Type = type;
                        integrationUser.UserId = user.Id;
                        integrationUser.IsForbidden = true;
                        integrationUser = _integrationUserRepo.Add(integrationUser);
                        _uow.Commit();

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
            catch (Exception e)
            {
                return new CreateIntegrationUserResult()
                {
                    State = CreateIntegrationUserState.Error
                };
            }
        }

        public UpdateIntegrationUserResult UpdateIntegrationUser(
            Int64 userId,
            String integrationUserName,
            String integrationUserDisplayName,
            IntegrationType type,
            Boolean isForbidden)
        {
            try
            {
                var user = GetUser(userId, true);
                if (user != null)
                {
                    var integrationUser = user.IntegrationUsers.FirstOrDefault(x => x.Type == type);
                    if (integrationUser != null)
                    {
                        integrationUser.Name = integrationUserName;
                        integrationUser.DisplayName = integrationUserDisplayName;
                        integrationUser.IsForbidden = isForbidden;
                        integrationUser = _integrationUserRepo.Update(integrationUser);
                        _uow.Commit();

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
            catch (Exception e)
            {
                return new UpdateIntegrationUserResult()
                {
                    State = UpdateIntegrationUserState.Error
                };
            }
        }

        public IntegrationUser GetIntegrationUser(
            IntegrationType integrationType,
            String integrationUserName)
        {
            return
                    _integrationUserRepo.Query()
                        .FirstOrDefault(x => x.Type == integrationType && x.Name == integrationUserName);
        }

        public IntegrationUser GetIntegrationUser(Int64 integrationUserId)
        {
            return _integrationUserRepo.GetById(integrationUserId);
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
