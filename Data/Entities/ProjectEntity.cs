﻿

namespace Data.Entities;

public class ProjectEntity
{
    public int Id {  get; set; }
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = "Not started";
    

    public int EmployeeId { get; set; }
    public EmployeeEntity? Employee { get; set; }
    public decimal TotalPrice => Employee?.Price ?? 0;


    public int CustomerId   { get; set; }
    public CustomerEntity? Customer { get; set; } 

    public int ServiceId { get; set; }
    public ServiceEntity?  Service { get; set; }
}
