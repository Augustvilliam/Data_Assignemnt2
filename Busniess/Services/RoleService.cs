

using Busniess.Interface;
using Data.Entities;
using Data.Interface;
using Data.Repositories;

namespace Busniess.Services;

public class RoleService : IRoleService
{
    private readonly IGenericRepository<RoleEntity> _roleRepository;

    public RoleService(IGenericRepository<RoleEntity> roleRepository)
    {
        _roleRepository = roleRepository;
    }


    public async Task<List<RoleEntity>> GetAllRolesAsync()
    {
        return await _roleRepository.GetAllAsync();

    }
    public async Task<RoleEntity?> GetRoleByIdAsync(int id)
    {
        return await _roleRepository.GetByIdAsync(id);
    }

    public async Task AddRoleAsync(RoleEntity role)
    {
        await _roleRepository.AddAsync(role);
    }

    public async Task UpdateRoleAsync(RoleEntity role)
    {
        await _roleRepository.UpdateAsync(role);
    }
    public async Task DeleteRoleAsync(int id)
    {
        await _roleRepository.DeleteAsync(id);
    }


}
