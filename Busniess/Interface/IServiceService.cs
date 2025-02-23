using Busniess.Dtos;

namespace Busniess.Interface
{
    public interface IServiceService //Servies blev inte riktigt som jag tänkte mig i slutändan. Tyckte det blev övermäktigt om man OCKSÅ skulle göra services i appen, så att det finns CRUD inlagt är bara för att jag copy pastade Customers Crud och bytte namn på skiten. 
    {
        Task AddService(ServiceDto service);
        Task DeleteServiceAsync(int id);
        Task<List<ServiceDto>> GetAllServicesAsync();
        Task<ServiceDto?> GetServiceByIdAsync(int id);
        Task UpdateServiceAsync(ServiceDto service);
    }
}