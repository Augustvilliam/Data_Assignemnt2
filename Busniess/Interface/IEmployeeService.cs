using Data.Entities;

namespace Busniess.Interface
{
    public interface IEmployeeService
    {
        Task AddEmployee(EmployeeEntity project);
        Task DeleteEmployeeAsync(int id);
        Task<List<EmployeeEntity>> GetAllEmployeesAsaync();
        Task<EmployeeEntity?> GetEmployeeByIdAsync(int id);
        Task UpdateEmployeeAsync(EmployeeEntity project);
    }
}