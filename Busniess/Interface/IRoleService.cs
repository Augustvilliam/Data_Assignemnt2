
using Data.Entities;

namespace Busniess.Interface
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(int id);
        Task AddRoleAsync(RoleDto role);
        Task UpdateRoleAsync(RoleDto role);
        Task DeleteRoleAsync(int id);
    }
}
