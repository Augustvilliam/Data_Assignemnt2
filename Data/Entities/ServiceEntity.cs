
namespace Data.Entities;

public class ServiceEntity
{
    public int Id { get; set; } 
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public List<ProjectEntity> Projects { get; set; } = new();

}
