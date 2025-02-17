

using Data.Entities;

namespace Busniess.Factories;

public class ProjectFactory
{
    public static ProjectEntity CreateProject(string name, string description, DateTime startDate, DateTime endDate, int customerId, int empoloyeeId, int serviceId)
    {
        return new ProjectEntity()
        {
            Name = name,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            CustomerId = customerId,
            EmployeeId = empoloyeeId,
            ServiceId = serviceId,
            Status = "Not started"
        };
    }
}
