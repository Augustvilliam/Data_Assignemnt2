using Busniess.Dtos;

namespace Busniess.Interface
{
    public interface IEmployeeService
    {
        Task AddEmployee(EmployeeDto dto);
        Task DeleteEmployeeAsync(int id);
        Task<List<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
        Task UpdateEmployeeAsync(EmployeeDto project);
    }
}