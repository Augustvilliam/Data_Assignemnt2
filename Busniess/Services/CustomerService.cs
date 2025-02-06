
using Busniess.Interface;
using Data.Entities;
using Data.Interface;

namespace Busniess.Services;

public class CustomerService : ICustomerService
{
    private readonly IGenericRepository<CustomerEntity> _customerRepository;

    public CustomerService(IGenericRepository<CustomerEntity> customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task AddCustomersAsync(CustomerEntity customer)
    {
        await _customerRepository.AddAsync(customer);
    }
    public async Task<List<CustomerEntity>> GetAllCustomersAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    public async Task<CustomerEntity?> GetCustomersByIdAsync(int id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }

    public async Task UpdateCustomersAsync(CustomerEntity customer)
    {
        await _customerRepository.UpdateAsync(customer);
    }

    public async Task DeleteCustomersAsync(int id)
    {
        await _customerRepository.DeleteAsync(id);
    }
}
