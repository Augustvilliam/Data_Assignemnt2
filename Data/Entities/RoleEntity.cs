
namespace Data.Entities;

public class RoleEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;  
    public decimal Price { get; set; }

    public List<EmployeeEntity> Employees { get; set; } = new();
}
