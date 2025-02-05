
using Data.Entities;

namespace Busniess.Interface
{
    public interface IRoleService
    {
        Task<List<RoleEntity>> GetAllRolesAsync();
        Task<RoleEntity?> GetRoleByIdAsync(int id);
        Task AddRoleAsync(RoleEntity role);
        Task UpdateRoleAsync(RoleEntity role);
        Task DeleteRoleAsync(int id);
    }
}
