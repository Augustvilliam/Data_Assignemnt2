using Data.Entities;

namespace Busniess.Interface
{
    public interface IServiceService
    {
        Task AddService(ServiceEntity project);
        Task DeleteServiceAsync(int id);
        Task<List<ServiceEntity>> GetAllServicesAsync();
        Task<ServiceEntity?> GetServiceByIdAsync(int id);
        Task UpdateServiceAsync(ServiceEntity project);
    }
}