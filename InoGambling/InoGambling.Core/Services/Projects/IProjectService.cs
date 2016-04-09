using System;
using System.Threading.Tasks;
using InoGambling.Core.Services.Projects.Models;
using InoGambling.Data.Model;

namespace InoGambling.Core.Services.Projects
{
    public interface IProjectService
    {
        Task<CreateProjectResult> CreateProject(
            IntegrationType integration, 
            String projectShortName, 
            String projectName);
    }
}
