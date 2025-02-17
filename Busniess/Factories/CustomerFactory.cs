

using Data.Entities;

namespace Busniess.Factories;

public class CustomerFactory
{
    public static CustomerEntity CreateCustomer(string firstName, string lastName, string email, string phoneNumber, List<ProjectEntity>? projects = null)
    {
        return new CustomerEntity
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phoneNumber,
            Projects = projects ?? new List<ProjectEntity>()
        };
    }
}
