

using Busniess.Dtos;
using Data.Entities;

namespace Busniess.Factories;

public class ProjectFactory
{
    //  Skapar en ProjectEntity från en ProjectDto
    public static ProjectEntity CreateProject(ProjectDto dto)
    {
        return new ProjectEntity
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            CustomerId = dto.CustomerId,
            EmployeeId = dto.EmployeeId,
            ServiceId = dto.ServiceId,
            Status = dto.Status
        };
    }

    // 🆕 Skapar en ProjectDto från en ProjectEntity
    public static ProjectDto CreateDto(ProjectEntity entity)
    {
        return new ProjectDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            CustomerId = entity.CustomerId,
            CustomerName = entity.Customer?.FirstName + " " + entity.Customer?.LastName ?? "Unknown",
            EmployeeId = entity.EmployeeId,
            EmployeeName = entity.Employee?.FirstName + " " + entity.Employee?.LastName ?? "Unknown",
            ServiceId = entity.ServiceId,
            ServiceName = entity.Service?.Name ?? "Unknown",
            Status = entity.Status,
            TotalPrice = (entity.Service?.BasePrice ?? 0) * (entity.Service?.EstimatedHours ?? 0) // 🛑 Anpassat!
        };
    }
}
