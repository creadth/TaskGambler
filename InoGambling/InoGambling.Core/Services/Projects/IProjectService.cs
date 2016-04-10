using System;
using System.Threading.Tasks;
using InoGambling.Core.Services.Projects.Models;
using InoGambling.Data.Model;

namespace InoGambling.Core.Services.Projects
{
    public interface IProjectService
    {
        Project GetProject(
            IntegrationType integrationType, 
            String projectShortId);

        CreateProjectResult CreateProject(
            IntegrationType integrationType, 
            String projectShortId);
    }
}
