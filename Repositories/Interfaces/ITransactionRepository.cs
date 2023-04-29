using AgroExpressAPI.Entities;

namespace AgroExpressAPI.Repositories.Interfaces;
    public interface ITransactionRepository
    {
        Task<Transaction> CreateAsync(Transaction transaction);
        Transaction GetByReferenceNumberAsync(string refNumber);
        Task<IEnumerable<Transaction>> GetByEmailAsync(string userEmail);
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task SaveChangesAsync();
        Task SaveChanges();
    }
