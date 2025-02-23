
using Busniess.Dtos;
using Data.Context;
using Data.Entities;

namespace Busniess.Factories;

public class EmployeeFactory
{
    // mappar en EmployeeEntity från Dto
    public static EmployeeEntity CreateEmployee(EmployeeDto dto, DataDbContext context)
    {
        int roleId = dto.RoleId > 0 ? dto.RoleId : 1; //Se till att RoleId aldrig är 0

        var role = context.Roles.Find(roleId); //letar redo på roller. gjord med chatGPT
        if (role == null)
        {
            throw new InvalidOperationException($"Role with Id {roleId} not found.");
        }

        var serviceIds = dto.Services.Select(s => s.Id).ToList();
        var services = context.Services.Where(s => serviceIds.Contains(s.Id)).ToList();


        return new EmployeeEntity //mappar enititen 
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            RoleId = role.Id,
            Role = role,
            Services = services,
            Projects = dto.Projects.Select(p => new ProjectEntity { Id = p.Id, Name = p.Name }).ToList()
        };
    }
    

    // mappar en EmployeeDto frå entitet.
    public static EmployeeDto CreateDto(EmployeeEntity entity)
    {
        return new EmployeeDto //mappar dton
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            PhoneNumber = entity.PhoneNumber,
            RoleId = entity.RoleId,
            RoleName = entity.Role?.Name ?? string.Empty,
            HourlyRate = entity.Role?.Price ?? 0,
            Services = entity.Services?.Select(s => new ServiceDto { Id = s.Id, Name = s.Name }).ToList() ?? new List<ServiceDto>(),
            Projects = entity.Projects?.Select(p => new ProjectSummaryDto { Id = p.Id, Name = p.Name }).ToList() ?? new List<ProjectSummaryDto>()
        };
    }
}
