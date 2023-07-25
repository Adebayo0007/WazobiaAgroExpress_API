using AgroExpressAPI.ApplicationContext;
using AgroExpressAPI.Entities;
using AgroExpressAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgroExpressAPI.Repositories.Implementations;
public class UserRepository : IUserRepository
{
     private readonly ApplicationDbContext _applicationDbContext;
        public UserRepository(ApplicationDbContext applicationDbContext) =>
            _applicationDbContext = applicationDbContext;
        public async Task<User> CreateAsync(User user)
        {
           await _applicationDbContext.Users.AddAsync(user);
           await SaveChangesAsync();
           return user;
        }

        public async Task Delete(User user)
        {
            _applicationDbContext.Users.Update(user);
            _applicationDbContext.SaveChanges();
        }

        public async Task<bool> ExistByEmailAsync(string userEmail) =>
          await _applicationDbContext.Users.AnyAsync(a => a.Email == userEmail);
        
        public async Task<bool> ExistByPasswordAsync(string userPassword) =>
              await _applicationDbContext.Users.AnyAsync(a => a.Password == userPassword);

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _applicationDbContext.Users
                 .Include(u => u.Farmer)
                 .Include(u =>u.Admin)
                 .Include(u => u.Buyer)
                 .Include(u => u.Address)
                 .Where(u =>u.IsActive == true && u.Role != "Admin" || u.IsActive == false && u.Role != "Admin")
                 .ToListAsync();

        public Task<IEnumerable<User>> GetAllNonActiveAsync() =>
            throw new NotImplementedException();

        public User GetByEmailAsync(string userEmail) =>
             _applicationDbContext.Users
                .Include(a => a.Address)
                .Include(a => a.Buyer)
                .Include(a => a.Farmer)
                .Include(a => a.Admin)
                .SingleOrDefault(a => a.Email == userEmail);

        public User GetByIdAsync(string userId) =>
              _applicationDbContext.Users
                     .Include(a => a.Address)
                     .Include(a => a.Buyer)
                     .Include(a => a.Farmer)
                     .Include(a => a.Admin)
                    .SingleOrDefault(a => a.Id == userId);

        public async Task<IEnumerable<User>> PendingRegistration() =>
            await _applicationDbContext.Users.Include(u => u.Address).Where(u => u.IsRegistered == false).ToListAsync();

        public async Task<IEnumerable<User>> SearchUserByEmailOrUsername(string searchInput) =>
            await _applicationDbContext.Users.Include(u =>u.Address).Where(u => u.UserName.ToLower() == searchInput.ToLower().Trim() || u.Email.ToLower() == searchInput.ToLower().Trim()).ToListAsync();

        public User Update(User user)
        {
            _applicationDbContext.Users.Update(user);
             _applicationDbContext.SaveChanges();
            return user;
        }

          public async Task SaveChanges() =>
            _applicationDbContext.SaveChanges();

        public async Task SaveChangesAsync() =>
            await _applicationDbContext.SaveChangesAsync();

    public Task<User> GetByName(string name) =>
    _applicationDbContext.Users.SingleOrDefaultAsync(u => u.Name == name);
    
}
