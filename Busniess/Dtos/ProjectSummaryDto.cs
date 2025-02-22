

namespace Busniess.Dtos;

public class ProjectSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = "Not started";
    public decimal TotalPrice { get; set; } = 0;
}
