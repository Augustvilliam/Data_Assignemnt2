
using Busniess.Interface;
using Data.Entities;
using Data.Interface;

namespace Busniess.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IGenericRepository<EmployeeEntity> _employeeRespitory;

    public EmployeeService(IGenericRepository<EmployeeEntity> employeeRepository)
    {
        _employeeRespitory = employeeRepository;
    }

    public async Task AddEmployee(EmployeeEntity project)
    {
        await _employeeRespitory.AddAsync(project);
    }
    public async Task<List<EmployeeEntity>> GetAllEmployeesAsaync()
    {
        return await _employeeRespitory.GetAllAsync();
    }

    public async Task<EmployeeEntity?> GetEmployeeByIdAsync(int id)
    {
        return await _employeeRespitory.GetByIdAsync(id);
    }

    public async Task UpdateEmployeeAsync(EmployeeEntity project)
    {
        await _employeeRespitory.UpdateAsync(project);
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        await _employeeRespitory.DeleteAsync(id);
    }
}
