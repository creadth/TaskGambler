using InoGambling.Data.Model;

namespace InoGambling.Data.Repositories.Impl
{
    public class BetRepositoryImpl : RepositoryImpl<Bet>, IBetRepository
    {
        public BetRepositoryImpl(IDataContextFactory factory) : base(factory)
        {
        }
    }
}
