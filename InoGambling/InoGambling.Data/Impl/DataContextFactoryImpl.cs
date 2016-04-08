namespace InoGambling.Data.Impl
{
    public class DataContextFactoryImpl : IDataContextFactory
    {
        public InoGamblingDbContext GetDbContext()
        {
            return Context;
        }

        protected InoGamblingDbContext Context {
            get
            {
                if (_context == null)
                {
                    _context = new InoGamblingDbContext();
                }
                return _context;
            }
        }
        private InoGamblingDbContext _context;
    }
}
