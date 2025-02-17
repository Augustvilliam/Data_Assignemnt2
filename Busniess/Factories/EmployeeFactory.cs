
using Data.Entities;

namespace Busniess.Factories;

public class EmployeeFactory
{
    public static EmployeeEntity CreateEmployee(string firstName,string lastName, string email, string phoneNumber, RoleEntity role, List<ServiceEntity>? services = null)
    {
        return new EmployeeEntity
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phoneNumber,
            RoleId = role.Id,
            Role = role,
            Services = services ?? new List<ServiceEntity>()
        };
    }
}
