
using System.Runtime.InteropServices;
using Busniess.Interface;
using Data.Context;
using Data.Entities;
using Data.Interface;
using Microsoft.EntityFrameworkCore;

namespace Busniess.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IGenericRepository<EmployeeEntity> _employeeRepository;
    private readonly DataDbContext _context;

    public EmployeeService(IGenericRepository<EmployeeEntity> employeeRepository, DataDbContext context)
    {
        _employeeRepository = employeeRepository;
        _context = context;
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

    public async Task UpdateEmployeeAsync(EmployeeEntity employee) //kopplar bort så tabellen kan uppdateras utan Idkrock. Gjort med hjälp av chatGPT
    {
        var trackedEntity = await _context.Employees
            .Include(e => e.Services)
            .FirstOrDefaultAsync(e => e.Id == employee.Id);
        if (trackedEntity != null)
        {
            _context.Entry(trackedEntity).State = EntityState.Detached;
        }
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        await _employeeRepository.DeleteAsync(id);
    }
}
