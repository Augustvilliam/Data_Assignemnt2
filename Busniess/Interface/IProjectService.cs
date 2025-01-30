using Data.Entities;

namespace Busniess.Interface
{
    public interface IProjectService
    {
        Task AddProject(ProjectEntity project);
        Task DeleteProjectAsync(int id);
        Task<List<ProjectEntity>> GetAllProjectsAsync();
        Task<ProjectEntity?> GetProjectByIdAsync(int id);
        Task UpdateProjectAsync(ProjectEntity project);
    }
}