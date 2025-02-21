namespace Data.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task DeleteAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task RollbackTransactionAsync();
        Task UpdateAsync(T entity);
    }
}