using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InoGambling.Data.Repositories;

namespace InoGambling.Data.Impl
{
    public class UnitOfWorkImpl : IUnitOfWork
    {
        public UnitOfWorkImpl(IDataContextFactory factory)
        {
            _factory = factory;
        }

        public TRepository GetRepository<TRepository>() where TRepository : class 
        {
            //TODO: resolve with Unity
            return null;
        }

        public Int32 Commit()
        {
            return _factory.GetDbContext().SaveChanges();
        }

        public async Task<Int32> CommitAsync()
        {
            return await _factory.GetDbContext().SaveChangesAsync();
        }

        private IDataContextFactory _factory;
    }
}
