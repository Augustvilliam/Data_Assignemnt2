

namespace Data.Entities;

public class EmployeeEntity
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

    public int RoleId { get; set; } 
    public RoleEntity? Role { get; set; } = null!; //genererad av chatgpt

    public decimal Price => Role?.Price ?? 0; //genererad av chatgpt

    public List<ProjectEntity> Projects { get; set; } = new(); //genererad av chatgpt rellation till Projeckt.
    public List<ServiceEntity> Services { get; set; } = new(); //genererad av chatgpt. relation till Services. 
}
