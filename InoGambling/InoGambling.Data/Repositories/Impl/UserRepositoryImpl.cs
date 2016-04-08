using InoGambling.Data.Models;

namespace InoGambling.Data.Repositories.Impl
{
    public class UserRepositoryImpl : RepositoryImpl<User>, IUserRepository
    {
        public UserRepositoryImpl(IDataContextFactory factory) : base(factory)
        {
        }
    }
}
