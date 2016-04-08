using InoGambling.Data.Models;

namespace InoGambling.Data.Repositories.Impl
{
    public class ProjectRepositoryImpl : RepositoryImpl<Project>, IProjectRepository
    {
        public ProjectRepositoryImpl(IDataContextFactory factory) : base(factory)
        {
        }
    }
}
