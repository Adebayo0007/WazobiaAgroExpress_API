using AgroExpressAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgroExpressAPI.ApplicationContext;
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)
        {
            
        }

         public DbSet<Admin> Admins { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<RequestedProduct> RequestedProducts { get; set; }
        public DbSet<User> Users { get; set; }

           protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>().HasData(
                new Address{
                        Id = "cc7578e3-52a9-49e9-9788-2da54df19f38",
                        FullAddress = "10,Abayomi street,Ipaja,lagos",
                        LocalGovernment = "Alimosho",
                        State = "Lagos"
                });

                modelBuilder.Entity<User>().HasData(
                  new User
                {
                       Id = "37846734-732e-4149-8cec-6f43d1eb3f60",
                       AddressId = "cc7578e3-52a9-49e9-9788-2da54df19f38",
                       Role = "Admin",
                       IsActive = true,
                       Password = BCrypt.Net.BCrypt.HashPassword("Admin0001"),
                       UserName = "Modrator",
                       Name = "Adebayo Addullah",
                       PhoneNumber = "08087054632",
                       Gender =  "Male",
                       Email = "tijaniadebayoabdllahi@gmail.com",
                       DateCreated = DateTime.Now,
                       IsRegistered = true,
                       Haspaid = true,
                       Due = true
                    
                }
              );


                 modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = "37846734-732e-4149-8cec-6f43d1eb3f60",
                    UserId = "37846734-732e-4149-8cec-6f43d1eb3f60",
              
                }
            );
        }

        
    }
