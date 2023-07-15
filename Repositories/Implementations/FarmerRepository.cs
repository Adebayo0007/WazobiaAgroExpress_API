using AgroExpressAPI.ApplicationContext;
using AgroExpressAPI.Entities;
using AgroExpressAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgroExpressAPI.Repositories.Implementations;
    public class FarmerRepository : IFarmerRepository
    {
         private readonly ApplicationDbContext _applicationDbContext;
        public  FarmerRepository(ApplicationDbContext applicationContext) =>
            _applicationDbContext = applicationContext;
        public async Task<Farmer> CreateAsync(Farmer farmer)
        {
            await _applicationDbContext.Farmers.AddAsync(farmer);
           await SaveChangesAsync();
           return farmer;
        }

        public async Task Delete(Farmer farmer)
        {
             _applicationDbContext.Farmers.Update(farmer);
             await _applicationDbContext.SaveChangesAsync();
        }

        public async Task FarmerMonthlyDueUpdate()
        {
              var farmers = await _applicationDbContext.Farmers
                      .Include(a => a.User)
                      .Where(a => a.User.IsActive == true && a.User.Role == "Farmer")
                      .ToListAsync();
              foreach(var farmer in farmers)
              {
                farmer.User.IsActive = false;
              }
              _applicationDbContext.UpdateRange(farmers);
        }

        public async Task<IEnumerable<Farmer>> GetAllAsync() =>
             await _applicationDbContext.Farmers
                  .Include(a => a.User)
                  .ThenInclude(a => a.Address)
                  .Where(a => a.User.IsActive == true && a.User.Role == "Farmer")
                 .ToListAsync();

        public async Task<IEnumerable<Farmer>> GetAllNonActiveAsync() =>
             await _applicationDbContext.Farmers
                    .Include(a => a.User)
                    .ThenInclude(a => a.Address)
                    .Where(a => a.User.IsActive == false && a.User.Role == "Farmer")
                    .ToListAsync();

        public Farmer GetByEmailAsync(string farmerEmail) =>
             _applicationDbContext.Farmers
                    .Include(a => a.User)
                    .ThenInclude(a => a.Address)
                    .SingleOrDefault(a => a.User.Email == farmerEmail);

        public Farmer GetByIdAsync(string farmerId) =>
              _applicationDbContext.Farmers.Include(a => a.User).ThenInclude(a => a.Address).SingleOrDefault(a => a.Id == farmerId);

        public async Task<Farmer> GetFarmer(string userId) =>
              _applicationDbContext.Farmers.Include(f => f.User).SingleOrDefault(f => f.UserId == userId);
        
        public async Task<IEnumerable<Farmer>> SearchFarmerByEmailOrUsername(string searchInput) =>
             await _applicationDbContext.Farmers
                     .Include(b => b.User)
                     .ThenInclude(b => b.Address)
                     .Where(b => b.User.Email.ToLower() == searchInput.ToLower().Trim() || b.User.UserName.ToLower() == searchInput.ToLower().Trim())
                     .ToListAsync();

        public Farmer Update(Farmer farmer)
        {
            _applicationDbContext.Farmers.Update(farmer);
            _applicationDbContext.SaveChanges();
            return farmer;
        }

        public async Task SaveChanges() =>
           _applicationDbContext.SaveChanges();

        public async Task SaveChangesAsync() =>
            await _applicationDbContext.SaveChangesAsync();
    }