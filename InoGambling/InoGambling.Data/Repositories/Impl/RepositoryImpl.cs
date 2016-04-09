using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using InoGambling.Data.Model;

namespace InoGambling.Data.Repositories.Impl
{
    public abstract class RepositoryImpl<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        protected RepositoryImpl(IDataContextFactory factory)
        {
            _factory = factory;
        }

        public async Task<TEntity> GetById(Int64 id)
        {
            return await ContextFactory.GetDbContext().Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<TEntity> Query()
        {
            return ContextFactory.GetDbContext().Set<TEntity>();
        }

        public TEntity Create()
        {
            return ContextFactory.GetDbContext().Set<TEntity>().Create();
        }

        public TEntity Add(TEntity entity)
        {
            return ContextFactory.GetDbContext().Set<TEntity>().Add(entity);
        }

        public TEntity Update(TEntity entity)
        {
            entity = ContextFactory.GetDbContext().Set<TEntity>().Attach(entity);
            ContextFactory.GetDbContext().Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public TEntity Remove(TEntity entity)
        {
            return ContextFactory.GetDbContext().Set<TEntity>().Remove(entity);
        }

        public IEnumerable<TEntity> Remove(IEnumerable<TEntity> entities)
        {
            return ContextFactory.GetDbContext().Set<TEntity>().RemoveRange(entities);
        }

        protected IDataContextFactory ContextFactory => _factory;
        private readonly IDataContextFactory _factory;
    }
}
