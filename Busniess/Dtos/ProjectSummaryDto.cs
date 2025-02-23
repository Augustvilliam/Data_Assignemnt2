

namespace Busniess.Dtos;

public class ProjectSummaryDto //Helt generrad av ChatGpt .4o
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = "Not started";
    public decimal TotalPrice { get; set; } = 0;
}
