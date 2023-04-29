using AgroExpressAPI.Entities;

namespace AgroExpressAPI.Repositories.Interfaces;
    public interface IFarmerRepository : IBaseRepository<Farmer>
    {
        Task<IEnumerable<Farmer>> SearchFarmerByEmailOrUsername(string searchInput); 
        Task<Farmer> GetFarmer(string userId); 
        Task FarmerMonthlyDueUpdate();
    }
