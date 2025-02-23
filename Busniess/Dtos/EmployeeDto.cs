

namespace Busniess.Dtos;

public class EmployeeDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int RoleId { get; set; } = 1;
    public string RoleName { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; }
    public List<ServiceDto> Services { get; set; } = new();
    public List<ProjectSummaryDto> Projects { get; set; } = new();
}
