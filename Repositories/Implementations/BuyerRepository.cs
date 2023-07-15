using AgroExpressAPI.ApplicationContext;
using AgroExpressAPI.Entities;
using AgroExpressAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgroExpressAPI.Repositories.Implementations;
    public class BuyerRepository : IBuyerRepository
    {
         private readonly ApplicationDbContext _applicationDbContext;
        public BuyerRepository(ApplicationDbContext applicationContext) =>
            _applicationDbContext = applicationContext;
        public async  Task<Buyer> CreateAsync(Buyer buyer)
        {
              await _applicationDbContext.Buyers.AddAsync(buyer);
              await SaveChangesAsync();
              return buyer;
        }

        public async Task Delete(Buyer buyer) =>
            await _applicationDbContext.SaveChangesAsync();

        public async Task<IEnumerable<Buyer>> GetAllAsync() =>
             await _applicationDbContext.Buyers
                    .Include(a => a.User)
                    .ThenInclude(a => a.Address)
                    .Where(a => a.User.IsActive == true && a.User.Role == "Buyer")
                    .ToListAsync();

        public async Task<IEnumerable<Buyer>> GetAllNonActiveAsync() => 
           await _applicationDbContext.Buyers
                   .Include(a => a.User)
                   .ThenInclude(a => a.Address)
                   .Where(a => a.User.IsActive == false && a.User.Role == "Buyer")
                   .ToListAsync();

        public Buyer GetByEmailAsync(string buyerEmail) =>
        _applicationDbContext.Buyers.Include(a => a.User).ThenInclude(a => a.Address).SingleOrDefault(a => a.User.Email == buyerEmail);

        public Buyer GetByIdAsync(string buyerId) =>
              _applicationDbContext.Buyers.Include(a => a.User).ThenInclude(a => a.Address).SingleOrDefault(a => a.Id == buyerId);


        public async Task<IEnumerable<Buyer>> SearchBuyerByEmailOrUsername(string searchInput) => 
              await _applicationDbContext.Buyers
                    .Include(b => b.User)
                    .ThenInclude(b => b.Address)
                    .Where(b => b.User.Email.ToLower()  == searchInput.ToLower().Trim() || b.User.UserName.ToLower() == searchInput.ToLower().Trim())
                    .ToListAsync();
       

        public Buyer Update(Buyer buyer)
        {
             _applicationDbContext.Buyers.Update(buyer);
             _applicationDbContext.SaveChanges();
            return buyer;
        }
         public async Task SaveChangesAsync() =>
           await _applicationDbContext.SaveChangesAsync();

    public async Task SaveChanges() =>
        _applicationDbContext.SaveChanges();
    }