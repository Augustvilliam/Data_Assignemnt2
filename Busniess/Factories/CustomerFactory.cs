

using Busniess.Dtos;
using Data.Entities;

namespace Busniess.Factories;


//Factory för Cusomer, båda tillsnyggade med chatgpt.
public class CustomerFactory
{
    public static CustomerEntity CreateCustomer(CustomerDto dto)
    {
        return new CustomerEntity //mappar en CustomerEntity
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Projects = dto.Projects?.Select(p => new ProjectEntity { Id = p.Id, Name = p.Name }).ToList() ?? new List<ProjectEntity>()
        };
    }

    public static CustomerDto CreateDto(CustomerEntity entity)
    {
        return new CustomerDto //mappar en Dto från en entity
        { 
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            PhoneNumber = entity.PhoneNumber,
            Projects = entity.Projects.Select(p => new ProjectSummaryDto { Id = p.Id, Name = p.Name }).ToList()
        };
    }
}
