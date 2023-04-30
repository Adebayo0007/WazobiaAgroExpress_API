using AgroExpressAPI.Dtos;
using AgroExpressAPI.Dtos.RequestedProduct;

namespace AgroExpressAPI.Services.Interfaces;
    public interface IRequestedProductService
    {
          Task<BaseResponse<RequestedProductDto>> CreateRequstedProductAsync(string productId, CreateRequestedProductRequestModel requestedModelodel);   
           Task<BaseResponse<RequestedProductDto>> GetRequestedProductById(string productId); 
        Task<BaseResponse<OrderedRequestAndPendingRequest>> OrderedAndPendingProduct(string buyerEmail);  
        Task DeleteRequestedProduct(string productId); 
        Task ProductDelivered(string productId);
        Task<BaseResponse<RequestedProductDto>> ProductAccepted(string productId);
         Task<BaseResponse<IEnumerable<RequestedProductDto>>> MyRequests(string farmerId);
          Task NotDelivered(string productId);
    }
