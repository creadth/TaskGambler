using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using InoGambling.Core.Services.Projects.Models;
using InoGambling.Data;
using InoGambling.Data.Model;
using InoGambling.Data.Repositories;

namespace InoGambling.Core.Services.Projects.Impl
{
    public class ProjectServiceImpl : IProjectService
    {
        public ProjectServiceImpl(IProjectRepository projectRepo, IUnitOfWork uow)
        {
            _projectRepo = projectRepo;
            _uow = uow;
        }

        public Project GetProject(
            IntegrationType integrationType,
            String projectShortId)
        {
            return _projectRepo.Query().FirstOrDefault(x => x.ShortId == projectShortId && x.IntegrationType == integrationType);
        }

        public CreateProjectResult CreateProject(
            IntegrationType integrationType,
            String projectShortId)
        {
            try
            {
                var project = GetProject(integrationType, projectShortId);
                if (project == null)
                {
                    project = _projectRepo.Create();
                    project.IntegrationType = integrationType;
                    project.ShortId = projectShortId;
                    project = _projectRepo.Add(project);
                    _uow.Commit();
                    return new CreateProjectResult()
                    {
                        State = CreateProjectState.Ok,
                        Project = project
                    };
                }
                return new CreateProjectResult()
                {
                    State = CreateProjectState.ProjectExists
                };
            }
            catch (Exception e)
            {
                return new CreateProjectResult()
                {
                    State = CreateProjectState.Error
                };
            }
        }

        private readonly IProjectRepository _projectRepo;
        private readonly IUnitOfWork _uow;
    }
}
