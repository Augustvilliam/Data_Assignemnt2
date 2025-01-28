

using Busniess.Interface;
using Data.Entities;
using Data.Interface;

namespace Busniess.Services;

public class ProjectService : IProjectService
{
    private readonly IGenericRepository<ProjectEntity> _projectRepository;

    public ProjectService(IGenericRepository<ProjectEntity> projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task AddProject(ProjectEntity project)
    {
        await _projectRepository.AddAsync(project);
    }
    public async Task<List<ProjectEntity>> GetAllProjectsAsaync()
    {
        return await _projectRepository.GetAllAsync();
    }

    public async Task<ProjectEntity?> GetProjectByIdAsync(int id)
    {
        return await _projectRepository.GetByIdAsync(id);
    }

    public async Task UpdateProjectAsync(ProjectEntity project)
    {
        await _projectRepository.UpdateAsync(project);
    }

    public async Task DeletePorjectAsync(int id)
    {
        await _projectRepository.DeleteAsync(id);
    }


}
