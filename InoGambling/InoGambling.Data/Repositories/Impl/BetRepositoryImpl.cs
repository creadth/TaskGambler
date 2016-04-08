using InoGambling.Data.Models;

namespace InoGambling.Data.Repositories.Impl
{
    class BetRepositoryImpl : RepositoryImpl<Bet>, IBetRepository
    {
        public BetRepositoryImpl(IDataContextFactory factory) : base(factory)
        {
        }
    }
}
