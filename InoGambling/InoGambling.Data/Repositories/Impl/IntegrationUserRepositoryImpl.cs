using InoGambling.Data.Models;

namespace InoGambling.Data.Repositories.Impl
{
    class IntegrationUserRepositoryImpl : RepositoryImpl<IntegrationUser>, IIntegrationUserRepository
    {
        public IntegrationUserRepositoryImpl(IDataContextFactory factory) : base(factory)
        {
        }
    }
}
