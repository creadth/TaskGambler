using InoGambling.Data.Models;

namespace InoGambling.Data.Repositories.Impl
{
    class UserRepositoryImpl : RepositoryImpl<User>, IUserRepository
    {
        public UserRepositoryImpl(IDataContextFactory factory) : base(factory)
        {
        }
    }
}
