

using System.Diagnostics;
using Busniess.Dtos;
using Busniess.Factories;
using Busniess.Interface;
using Data.Context;
using Data.Entities;
using Data.Interface;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace Busniess.Services;

public class ProjectService : IProjectService
{
    private readonly IGenericRepository<ProjectEntity> _projectRepository;
    private readonly DataDbContext _context;

    public ProjectService(IGenericRepository<ProjectEntity> projectRepository, DataDbContext context)
    {
        _projectRepository = projectRepository;
        _context = context;
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
            var existingProject = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == dto.Id);

            if (existingProject == null)
            {
                throw new InvalidOperationException($"Project with Id {dto.Id} not found.");
            }

            existingProject.Name = dto.Name;
            existingProject.Description = dto.Description;
            existingProject.StartDate = dto.StartDate;
            existingProject.EndDate = dto.EndDate;
            existingProject.Status = dto.Status;

            existingProject.Customer = await _context.Customers.FindAsync(dto.CustomerId);
            existingProject.Employee = await _context.Employees.FindAsync(dto.EmployeeId);
            existingProject.Service = await _context.Services.FindAsync(dto.ServiceId);

            if (existingProject.Customer == null || existingProject.Employee == null || existingProject.Service == null)
            {
                throw new InvalidOperationException("One or more related entities not found.");
            }

            await _context.SaveChangesAsync();
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
