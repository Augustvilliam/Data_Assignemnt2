using Data.Entities;

namespace Busniess.Interface
{
    public interface IServiceService
    {
        Task AddService(ServiceEntity project);
        Task DeleteServiceAsync(int id);
        Task<List<ServiceEntity>> GetAllServiceAsaync();
        Task<ServiceEntity?> GetServiceByIdAsync(int id);
        Task UpdateServiceAsync(ServiceEntity project);
    }
}