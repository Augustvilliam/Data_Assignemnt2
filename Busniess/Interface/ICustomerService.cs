using Busniess.Dtos;

namespace Busniess.Interface
{
    public interface ICustomerService 
    {
        Task AddCustomersAsync(CustomerDto dto);
        Task DeleteCustomersAsync(int id);
        Task<List<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto?> GetCustomersByIdAsync(int id);
        Task UpdateCustomersAsync(CustomerDto customer);
    }
}