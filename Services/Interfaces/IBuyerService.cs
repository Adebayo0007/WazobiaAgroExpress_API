using AgroExpressAPI.Dtos;
using AgroExpressAPI.Dtos.AllBuyers;
using AgroExpressAPI.Dtos.Buyer;

namespace AgroExpressAPI.Services.Interfaces;
    public interface IBuyerService
    {
        Task<BaseResponse<BuyerDto>> CreateAsync(CreateBuyerRequestModel createBuyerModel);
        Task<BaseResponse<BuyerDto>> GetByIdAsync(string buyerId);
        Task<BaseResponse<BuyerDto>> GetByEmailAsync(string buyerEmail);
        Task<BaseResponse<IEnumerable<BuyerDto>>> GetAllAsync();
         Task<BaseResponse<ActiveAndNonActiveBuyers>> GetAllActiveAndNonActiveAsync();
        Task<BaseResponse<IEnumerable<BuyerDto>>> SearchBuyerByEmailOrUserName(string searchInput);
        Task<BaseResponse<BuyerDto>> UpdateAsync(UpdateBuyerRequestModel updateBuyerModel, string id);
        Task DeleteAsync(string buyerId);
    }
