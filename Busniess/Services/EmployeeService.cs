
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

    public async Task UpdateEmployeeAsync(EmployeeEntity employee)
    {
        var existingEmployee = await _context.Employees
            .Include(e => e.Services)
            .FirstOrDefaultAsync(e => e.Id == employee.Id);

        if (existingEmployee == null)
        {
            throw new InvalidOperationException("Employee not found.");
        }

        // 🛑 Rensa gamla tjänst-relationer för att undvika duplicering
        existingEmployee.Services.Clear();
        await _context.SaveChangesAsync(); // Spara ändringen direkt innan vi lägger till nya relationer

        // 🔄 Hämta uppdaterad lista över tjänster från databasen för att undvika duplicering
        var updatedServices = await _context.Services
            .Where(s => employee.Services.Select(es => es.Id).Contains(s.Id))
            .ToListAsync();

        // 🆕 Lägg till de nya tjänsterna och spara ändringen
        employee.Services = updatedServices;
        _context.Employees.Update(employee);

        await _context.SaveChangesAsync();
    }



    public async Task DeleteEmployeeAsync(int id)
    {
        await _employeeRepository.DeleteAsync(id);
    }
}
