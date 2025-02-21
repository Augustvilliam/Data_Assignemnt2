
using System.Diagnostics;
using Busniess.Dtos;
using Busniess.Factories;
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

    public async Task AddCustomersAsync(CustomerDto dto)
    {
        var customer = CustomerFactory.CreateCustomer(dto);

        await _customerRepository.BeginTransactionAsync();
        try
        {
            await _customerRepository.AddAsync(customer);
            await _customerRepository.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Error adding customer: {ex.Message}");
            await _customerRepository.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task<List<CustomerDto?>> GetAllCustomersAsync()
    {
        var customer = await _customerRepository.GetAllAsync();
        return customer.Select(CustomerFactory.CreateDto).ToList();
    }

    public async Task<CustomerDto?> GetCustomersByIdAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        return customer != null ? CustomerFactory.CreateDto(customer) : null;
    }

    public async Task UpdateCustomersAsync(CustomerDto dto)
    {
        await _customerRepository.BeginTransactionAsync();
        try
        {
            var customer = CustomerFactory.CreateCustomer(dto);
            customer.Id = dto.Id;

            await _customerRepository.UpdateAsync(customer);
            await _customerRepository.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($" Error updating customer: {ex.Message}");
            await _customerRepository.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task DeleteCustomersAsync(int id)
    {
        await _customerRepository.BeginTransactionAsync();
        try
        {
            await _customerRepository.DeleteAsync(id);
            await _customerRepository.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Error deleting customer: {ex.Message}");
            await _customerRepository.RollbackTransactionAsync();
            throw;
        }
    }
}
