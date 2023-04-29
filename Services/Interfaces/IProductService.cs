using AgroExpressAPI.Dtos;
using AgroExpressAPI.Dtos.Product;

namespace AgroExpressAPI.Services.Interfaces;
    public interface IProductService
    {
        Task<BaseResponse<ProductDto>> CreateProductAsync(CreateProductRequestModel product);
        Task<BaseResponse<ProductDto>> GetProductById(string productId);
        Task<BaseResponse<IEnumerable<ProductDto>>> GetAllFarmProductAsync();
        Task<BaseResponse<IEnumerable<ProductDto>>> GetAllFarmProductByLocationAsync();
        Task<BaseResponse<IEnumerable<ProductDto>>> GetFarmerFarmProductsByIdAsync();
        Task<BaseResponse<IEnumerable<ProductDto>>> SearchProductsByProductNameOrFarmerUserNameOrFarmerEmail(string searchInput);
        Task<BaseResponse<ProductDto>> UpdateProduct(UpdateProductRequestModel product, string productId);
        Task DeleteProduct(string productId);
        Task ThumbUp(string productId);
        Task ThumbDown(string productId);
        Task DeleteExpiredProducts(); 
    }
