using Busniess.Dtos;
using Busniess.Interface;
using Data.Entities;
using Data.Interface;

public class ServiceService : IServiceService //Mestadels copypast från chatGPT eftersom det blir bajsing med CRUD när jag byggde om till att anävnda DTO. 
{
    private readonly IGenericRepository<ServiceEntity> _serviceRepository;

    public ServiceService(IGenericRepository<ServiceEntity> serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task AddService(ServiceDto serviceDto)
    {
        var serviceEntity = new ServiceEntity
        {
            Id = serviceDto.Id,
            Name = serviceDto.Name
        };

        await _serviceRepository.AddAsync(serviceEntity);
    }

    public async Task<List<ServiceDto>> GetAllServicesAsync()
    {
        var services = await _serviceRepository.GetAllAsync();
        return services.Select(s => new ServiceDto { Id = s.Id, Name = s.Name }).ToList();
    }

    public async Task<ServiceDto?> GetServiceByIdAsync(int id)
    {
        var service = await _serviceRepository.GetByIdAsync(id);
        return service != null ? new ServiceDto { Id = service.Id, Name = service.Name } : null;
    }

    public async Task UpdateServiceAsync(ServiceDto serviceDto)
    {
        var existingService = await _serviceRepository.GetByIdAsync(serviceDto.Id);
        if (existingService != null)
        {
            existingService.Name = serviceDto.Name;
            await _serviceRepository.UpdateAsync(existingService);
        }
    }

    public async Task DeleteServiceAsync(int id)
    {
        await _serviceRepository.DeleteAsync(id);
    }
}
