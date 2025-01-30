using Data.Entities;

namespace Busniess.Interface
{
    public interface ICustomerService
    {
        Task AddCustomers(CustomerEntity customer);
        Task DeleteCustomersAsync(int id);
        Task<List<CustomerEntity>> GetAllCustomersAsync();
        Task<CustomerEntity?> GetCustomersByIdAsync(int id);
        Task UpdateCustomersAsync(CustomerEntity customer);
    }
}