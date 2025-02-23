
using System.Diagnostics;
using System.Runtime.InteropServices;
using Busniess.Dtos;
using Busniess.Factories;
using Busniess.Interface;
using Data.Context;
using Data.Entities;
using Data.Interface;
using Microsoft.EntityFrameworkCore;

namespace Busniess.Services;

public class EmployeeService : IEmployeeService //Delvis omgjord med chat GPT när jag införde DTOs i systmet. 
{
    private readonly IGenericRepository<EmployeeEntity> _employeeRepository;
    private readonly DataDbContext _context;

    public EmployeeService(IGenericRepository<EmployeeEntity> employeeRepository, DataDbContext context)
    {
        _employeeRepository = employeeRepository;
        _context = context;
    }

    public async Task AddEmployee(EmployeeDto dto)
    {
        await _employeeRepository.BeginTransactionAsync();
        try
        {
            var employee = EmployeeFactory.CreateEmployee(dto);

            // Hämta befintlig roll från databasen eftersom dessa är förkodade när databasen skapas. 
            employee.Role = await _context.Roles.FindAsync(dto.RoleId);
            if (employee.Role == null)
            {
                throw new InvalidOperationException($"Role with Id {dto.RoleId} not found.");
            }

            //  Hämta de befintliga tjänsterna från databasen, samma här då Services fick bli hårdkodade. 
            var selectedServiceIds = dto.Services.Select(s => s.Id).ToList();
            employee.Services = await _context.Services.Where(s => selectedServiceIds.Contains(s.Id)).ToListAsync();

            await _employeeRepository.AddAsync(employee);
            await _employeeRepository.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Error adding employee: {ex.Message}"); //Behåller Debugsen här även om det är oanvändbart i skaprt läge, för lat för att byta till en Alert eller logger.. 
            await _employeeRepository.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
    {
        var employees = await _context.Employees
             .Include(e => e.Role)
             .Include(e => e.Services)
             .Include(e => e.Projects)
             .ToListAsync();

        return employees.Select(EmployeeFactory.CreateDto).ToList();
    }
    public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
    {
        var employee = await _context.Employees
            .Include(e => e.Role)
            .Include(e => e.Services)
            .Include(e => e.Projects)
            .FirstOrDefaultAsync(e => e.Id == id);

        return employee != null ? EmployeeFactory.CreateDto(employee) : null;
    }
    public async Task UpdateEmployeeAsync(EmployeeDto dto)
    {
        await _employeeRepository.BeginTransactionAsync();
        try
        {
            var existingEmployee = await _context.Employees
                .Include(e => e.Services)
                .FirstOrDefaultAsync(e => e.Id == dto.Id);

            if (existingEmployee == null)
            {
                throw new InvalidOperationException("Employee not found.");
            }

            //  Rensa gamla tjänst-relationer eftersom Databasen fick spunk när man skulle spara en uppdaterad använde då den redan "okuperade" den primära nycklen.
            existingEmployee.Services.Clear();
            await _context.SaveChangesAsync();

            //  Hämta de befintliga tjänsterna från databasen efter att det är sparat
            var updatedServices = await _context.Services
                .Where(s => dto.Services.Select(es => es.Id).Contains(s.Id))
                .ToListAsync();

            //  Uppdatera den valda employees, denna är copypast från chatgpt
            existingEmployee.FirstName = dto.FirstName;
            existingEmployee.LastName = dto.LastName;
            existingEmployee.Email = dto.Email;
            existingEmployee.PhoneNumber = dto.PhoneNumber;
            existingEmployee.RoleId = dto.RoleId;
            existingEmployee.Role = await _context.Roles.FindAsync(dto.RoleId);
            existingEmployee.Services = updatedServices;

            await _context.SaveChangesAsync();
            await _employeeRepository.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Error updating employee: {ex.Message}"); //Behåller Debugsen här även om det är oanvändbart i skaprt läge, för lat för att byta till en Alert eller logger.. 
            await _employeeRepository.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        await _employeeRepository.BeginTransactionAsync();
        try
        {
            await _employeeRepository.DeleteAsync(id);
            await _employeeRepository.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Error deleting employee: {ex.Message}"); //Behåller Debugsen här även om det är oanvändbart i skaprt läge, för lat för att byta till en Alert eller logger.. 
            await _employeeRepository.RollbackTransactionAsync();
            throw;
        }
    }
}
