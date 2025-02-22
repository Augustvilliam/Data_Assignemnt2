
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
            dto.Id = customer.Id;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Error adding customer: {ex.Message}");
            await _customerRepository.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task<List<CustomerDto>> GetAllCustomersAsync()
    {
        var customers = await _customerRepository.GetAllAsync();

        return customers?
            .Select(c => CustomerFactory.CreateDto(c))
            .ToList() ?? new List<CustomerDto>();
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
            var existingCustomer = await _customerRepository.GetByIdAsync(dto.Id);
            if (existingCustomer == null)
            {
                throw new InvalidOperationException("Customer not found.");
            }

            existingCustomer.FirstName = dto.FirstName;
            existingCustomer.LastName = dto.LastName;
            existingCustomer.Email = dto.Email;
            existingCustomer.PhoneNumber = dto.PhoneNumber;

            await _customerRepository.UpdateAsync(existingCustomer);
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
