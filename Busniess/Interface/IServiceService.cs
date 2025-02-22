using Busniess.Dtos;
using Data.Entities;

namespace Busniess.Interface
{
    public interface IServiceService
    {
        Task AddService(ServiceDto service);
        Task DeleteServiceAsync(int id);
        Task<List<ServiceDto>> GetAllServicesAsync();
        Task<ServiceDto?> GetServiceByIdAsync(int id);
        Task UpdateServiceAsync(ServiceDto service);
    }
}