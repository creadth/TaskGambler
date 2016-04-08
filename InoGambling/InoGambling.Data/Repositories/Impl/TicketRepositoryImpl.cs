using InoGambling.Data.Models;

namespace InoGambling.Data.Repositories.Impl
{
    public class TicketRepositoryImpl : RepositoryImpl<Ticket>, ITicketRepository
    {
        public TicketRepositoryImpl(IDataContextFactory factory) : base(factory)
        {
        }
    }
}
