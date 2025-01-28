namespace Data.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task DeleteAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task UpdateAsync(T entity);
    }
}