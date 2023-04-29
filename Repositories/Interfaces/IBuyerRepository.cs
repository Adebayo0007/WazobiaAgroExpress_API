using AgroExpressAPI.Entities;

namespace AgroExpressAPI.Repositories.Interfaces;
    public interface IBuyerRepository : IBaseRepository<Buyer>
    {
        Task<IEnumerable<Buyer>> SearchBuyerByEmailOrUsername(string searchInput);
    }
