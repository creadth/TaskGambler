using System;
using System.Threading.Tasks;

namespace InoGambling.Data
{
    public interface IUnitOfWork
    {
        Int32 Commit();
        Task<Int32> CommitAsync();
    }
}
