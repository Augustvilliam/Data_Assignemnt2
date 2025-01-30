
using Busniess.Interface;
using Data.Entities;
using Data.Interface;

namespace Busniess.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IGenericRepository<EmployeeEntity> _employeeRepository;

    public EmployeeService(IGenericRepository<EmployeeEntity> employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task AddEmployee(EmployeeEntity project)
    {
        await _employeeRepository.AddAsync(project);
    }
    public async Task<List<EmployeeEntity>> GetAllEmployeesAsync()
    {
        return await _employeeRepository.GetAllAsync();
    }

    public async Task<EmployeeEntity?> GetEmployeeByIdAsync(int id)
    {
        return await _employeeRepository.GetByIdAsync(id);
    }

    public async Task UpdateEmployeeAsync(EmployeeEntity project)
    {
        await _employeeRepository.UpdateAsync(project);
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        await _employeeRepository.DeleteAsync(id);
    }
}
