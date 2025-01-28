using Data.Entities;

namespace Busniess.Interface
{
    public interface IProjectService
    {
        Task AddProject(ProjectEntity project);
        Task DeletePorjectAsync(int id);
        Task<List<ProjectEntity>> GetAllProjectsAsaync();
        Task<ProjectEntity?> GetProjectByIdAsync(int id);
        Task UpdateProjectAsync(ProjectEntity project);
    }
}