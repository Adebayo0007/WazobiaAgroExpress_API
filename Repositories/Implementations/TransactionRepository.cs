using AgroExpressAPI.ApplicationContext;
using AgroExpressAPI.Entities;
using AgroExpressAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgroExpressAPI.Repositories.Implementations;
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public TransactionRepository(ApplicationDbContext applicationContext)
        {
            _applicationDbContext = applicationContext;
        }

    public async Task<Transaction> CreateAsync(Transaction transaction)
    {
        var requestedProduct = await _applicationDbContext.Transactions.AddAsync(transaction);
              await _applicationDbContext.SaveChangesAsync();
            return transaction;
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync()
    {
        return await _applicationDbContext.Transactions.ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetByEmailAsync(string userEmail)
    {
       var transaction =  await _applicationDbContext.Transactions.Where(t => t.BuyerEmail == userEmail || t.FarmerEmail == userEmail).ToListAsync();
       return transaction;
    }

    public Transaction GetByReferenceNumberAsync(string refNumber)
    {
       return _applicationDbContext.Transactions.SingleOrDefault(t => t.ReferenceNumber == refNumber);
    }

    public async Task SaveChanges()
    {
        _applicationDbContext.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
      await  _applicationDbContext.SaveChangesAsync();
    }
}