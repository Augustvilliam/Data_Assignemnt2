

using System.Diagnostics;
using Busniess.Dtos;
using Busniess.Factories;
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

    public async Task AddProject(ProjectDto dto)
    {
        var project = ProjectFactory.CreateProject(dto);

        await _projectRepository.BeginTransactionAsync();
        try
        {
            await _projectRepository.AddAsync(project);
            await _projectRepository.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Error adding project: {ex.Message}");
            await _projectRepository.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task<List<ProjectDto>> GetAllProjectsAsync()
    {
        var projects = await _projectRepository.GetAllAsync();
        return projects.Select(ProjectFactory.CreateDto).ToList();
    }

    public async Task<ProjectDto?> GetProjectByIdAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        return project != null ? ProjectFactory.CreateDto(project) : null;
    }

    public async Task UpdateProjectAsync(ProjectDto dto)
    {
        await _projectRepository.BeginTransactionAsync();
        try
        {
            var project = ProjectFactory.CreateProject(dto);
            project.Id = dto.Id;

            await _projectRepository.UpdateAsync(project);
            await _projectRepository.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Error updating project: {ex.Message}");
            await _projectRepository.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task DeleteProjectAsync(int id)
    {
        await _projectRepository.BeginTransactionAsync();
        try
        {
            await _projectRepository.DeleteAsync(id);
            await _projectRepository.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Error deleting project: {ex.Message}");
            await _projectRepository.RollbackTransactionAsync();
            throw;
        }
    }


}
