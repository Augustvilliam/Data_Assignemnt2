
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ServiceEntity
{
    public int Id { get; set; } 
    public string Name { get; set; } = null!;
    public decimal BasePrice { get; set; }
    public int EstimatedHours { get; set; }
    public List<EmployeeEntity> Employees { get; set; } = new();
    public ServiceEntity() { }
}
