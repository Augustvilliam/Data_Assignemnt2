using Busniess.Dtos;

namespace Busniess.Interface
{
    public interface IProjectService
    {
        Task AddProject(ProjectDto dto);
        Task DeleteProjectAsync(int id);
        Task<List<ProjectDto>> GetAllProjectsAsync();
        Task<ProjectDto?> GetProjectByIdAsync(int id);
        Task UpdateProjectAsync(ProjectDto project);
    }
}