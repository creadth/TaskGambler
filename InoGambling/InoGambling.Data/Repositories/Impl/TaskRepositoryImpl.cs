using InoGambling.Data.Models;

namespace InoGambling.Data.Repositories.Impl
{
    class TaskRepositoryImpl : RepositoryImpl<Task>, ITaskRepository
    {
        public TaskRepositoryImpl(IDataContextFactory factory) : base(factory)
        {
        }
    }
}
