

using Busniess.Interface;
using Data.Entities;
using Data.Interface;

namespace Busniess.Services;

public class ServiceService : IServiceService
{
    private readonly IGenericRepository<ServiceEntity> _serviceRespitory;

    public ServiceService(IGenericRepository<ServiceEntity> serviceRepository)
    {
        _serviceRespitory = serviceRepository;
    }

    public async Task AddService(ServiceEntity project)
    {
        await _serviceRespitory.AddAsync(project);
    }
    public async Task<List<ServiceEntity>> GetAllServiceAsaync()
    {
        return await _serviceRespitory.GetAllAsync();
    }

    public async Task<ServiceEntity?> GetServiceByIdAsync(int id)
    {
        return await _serviceRespitory.GetByIdAsync(id);
    }

    public async Task UpdateServiceAsync(ServiceEntity project)
    {
        await _serviceRespitory.UpdateAsync(project);
    }

    public async Task DeleteServiceAsync(int id)
    {
        await _serviceRespitory.DeleteAsync(id);
    }
}
