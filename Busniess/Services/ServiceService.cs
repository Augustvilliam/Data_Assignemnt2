

using Busniess.Interface;
using Data.Entities;
using Data.Interface;

namespace Busniess.Services;

public class ServiceService : IServiceService
{
    private readonly IGenericRepository<ServiceEntity> _serviceRepository;

    public ServiceService(IGenericRepository<ServiceEntity> serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task AddService(ServiceEntity project)
    {
        await _serviceRepository.AddAsync(project);
    }
    public async Task<List<ServiceEntity>> GetAllServicesAsync()
    {
        return await _serviceRepository.GetAllAsync();
    }

    public async Task<ServiceEntity?> GetServiceByIdAsync(int id)
    {
        return await _serviceRepository.GetByIdAsync(id);
    }

    public async Task UpdateServiceAsync(ServiceEntity project)
    {
        await _serviceRepository.UpdateAsync(project);
    }

    public async Task DeleteServiceAsync(int id)
    {
        await _serviceRepository.DeleteAsync(id);
    }
}
