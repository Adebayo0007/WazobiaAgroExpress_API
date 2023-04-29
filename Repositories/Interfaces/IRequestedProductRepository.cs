using AgroExpressAPI.Entities;

namespace AgroExpressAPI.Repositories.Interfaces;
    public interface IRequestedProductRepository 
    {
        Task<RequestedProduct> CreateAsync(RequestedProduct product);
        RequestedProduct GetRequstedProductById(string requstedProductId);
        Task<IEnumerable<RequestedProduct>> GetRequestedProductsByFarmerEmailAsync(string farmerEmail);
         Task<IEnumerable<RequestedProduct>> GetOrderedProduct(string buyerEmail);
         Task<IEnumerable<RequestedProduct>> GetPendingProduct(string buyeEmail);
         Task<IEnumerable<RequestedProduct>> GetRequestedProductsByFarmerIdAsync(string farmerId);
         Task<IEnumerable<RequestedProduct>> GetRequestedProductsByBuyerIdAsync(string buyerId);
          Task<RequestedProduct> GetProductByProductIdAsync(string productId);
        RequestedProduct UpdateRequestedProduct(RequestedProduct requestedProduct);
        Task DeleteRequestedProduct(RequestedProduct requestedProduct);
    }
