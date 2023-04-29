namespace AgroExpressAPI.Repositories.Interfaces;
    public interface IBaseRepository<T>
    {
        Task<T> CreateAsync(T user);
        T GetByIdAsync(string userId);
        T GetByEmailAsync(string userEmail);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllNonActiveAsync();
        T Update(T user);
        Task Delete(T user);
        Task SaveChangesAsync();
        Task SaveChanges();
    }
