using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InoGambling.Data.Model;

namespace InoGambling.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        Task<TEntity> GetById(Int64 id);
        IQueryable<TEntity> Query();
        TEntity Create();
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        TEntity Remove(TEntity entity);
        IEnumerable<TEntity> Remove(IEnumerable<TEntity> entities);
    }
}
