using AgroExpressAPI.ApplicationContext;
using AgroExpressAPI.Entities;
using AgroExpressAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgroExpressAPI.Repositories.Implementations;
    public class ProductRepository : IProductRepository
    {
          private readonly ApplicationDbContext _applicationDbContext;
        public  ProductRepository(ApplicationDbContext applicationContext)
        {
            _applicationDbContext = applicationContext;
        }
        public async Task<Product> CreateAsync(Product product)
        {
            await _applicationDbContext.Products.AddAsync(product);
            await _applicationDbContext.SaveChangesAsync();
           return product;
        }

        public async Task DeleteProduct(Product product)
        {
             _applicationDbContext.Products.Remove(product);
             _applicationDbContext.SaveChanges();
        }

         public async Task DeleteExpiredProducts()
         {
             var expiredProduct = await _applicationDbContext.Products.Where(p =>p.AvailabilityDateTo.Date.Day  <  DateTime.Now.Date.Day).ToListAsync();
               _applicationDbContext.Products.RemoveRange(expiredProduct);  
              await _applicationDbContext.SaveChangesAsync();
         }

        public async Task<IEnumerable<Product>> GetAllFarmProductAsync()
        {
              return await _applicationDbContext.Products.Where(p => p.IsAvailable == true).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllFarmProductByLocationAsync(string buyerLocalGovernment, User user)
        {
              return await _applicationDbContext.Products.Where(p => p.ProductLocalGovernment == buyerLocalGovernment && p.ProductLocalGovernment == user.Address.LocalGovernment).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetFarmerFarmProductsByIdAsync(string farmerId)
        {
             return await _applicationDbContext.Products.Where(p => p.FarmerId == farmerId).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByFarmerEmailAsync(string farmerEmail)
        {
            return await _applicationDbContext.Products.Where(p => p.FarmerEmail == farmerEmail).ToListAsync();
        }

        public  async Task<IEnumerable<Product>> SearchProductsByProductNameOrFarmerUserNameOrFarmerEmail(string searchInput, User user)
        {
            var input = searchInput.ToLower().Trim();
            var searchedOutput = await _applicationDbContext.Products.Where(p => p.FarmerEmail.ToLower() == input && p.ProductLocalGovernment == user.Address.LocalGovernment || p.FarmerUserName.ToLower() == input && p.ProductLocalGovernment == user.Address.LocalGovernment || p.ProductName.ToLower() == input && p.ProductLocalGovernment == user.Address.LocalGovernment).ToListAsync();
            return searchedOutput;

        }

        public Product GetProductById(string productId)
        {
             return _applicationDbContext.Products.SingleOrDefault(p => p.Id == productId);
        }

        public Product UpdateProduct(Product product)
        {
             _applicationDbContext.Products.Update(product);
             _applicationDbContext.SaveChanges();
             return product;
        }
    }
