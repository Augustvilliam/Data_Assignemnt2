

using System.Diagnostics;
using Busniess.Interface;
using Data.Entities;
using Data.Interface;
using Data.Repositories;

namespace Busniess.Services;

public class RoleService : IRoleService //Delvis omgjord med chat GPT när jag införde DTOs i systmet. 
{
    private readonly IGenericRepository<RoleDto> _roleRepository;

    public RoleService(IGenericRepository<RoleDto> roleRepository)
    {
        _roleRepository = roleRepository;
    }


    public async Task<List<RoleDto>> GetAllRolesAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        Debug.WriteLine($"🟢 Hittade {roles.Count} roller i databasen.");
        return roles;
    }
    public async Task<RoleDto?> GetRoleByIdAsync(int id)
    {
        return await _roleRepository.GetByIdAsync(id);
    }

    public async Task AddRoleAsync(RoleDto role)
    {
        await _roleRepository.AddAsync(role);
    }

    public async Task UpdateRoleAsync(RoleDto role)
    {
        await _roleRepository.UpdateAsync(role);
    }
    public async Task DeleteRoleAsync(int id)
    {
        await _roleRepository.DeleteAsync(id);
    }


}
