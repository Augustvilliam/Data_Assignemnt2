
using Busniess.Dtos;
using Data.Entities;

namespace Busniess.Factories;

public class EmployeeFactory
{
    // mappar en EmployeeEntity från Dto
    public static EmployeeEntity CreateEmployee(EmployeeDto dto)
    {
        return new EmployeeEntity
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            RoleId = dto.RoleId,
            Role = new RoleEntity
            {
                Id = dto.RoleId,
                Name = dto.RoleName,
                Price = dto.HourlyRate
            },
            Services = dto.Services.Select(s => new ServiceEntity { Id = s.Id, Name = s.Name }).ToList(),
            Projects = dto.Projects.Select(p => new ProjectEntity { Id = p.Id, Name = p.Name }).ToList()
        };
    }

    // mappar en EmployeeDto frå entitet.
    public static EmployeeDto CreateDto(EmployeeEntity entity)
    {
        return new EmployeeDto
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
