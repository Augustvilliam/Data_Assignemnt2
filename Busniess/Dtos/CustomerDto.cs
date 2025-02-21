﻿

using Busniess.Dtos;

namespace Busniess.Dtos;

public class CustomerDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public List<ProjectSummaryDto> Projects { get; set; } = new();
}
