using AgroExpressAPI.Dtos;
using AgroExpressAPI.Dtos.AllFarmers;
using AgroExpressAPI.Dtos.Farmer;

namespace AgroExpressAPI.Services.Interfaces;
    public interface IFarmerService
    {
         Task<BaseResponse<FarmerDto>> CreateAsync(CreateFarmerRequestModel createFarmerModel);
        Task<BaseResponse<FarmerDto>> GetByIdAsync(string farmerId);
        Task<BaseResponse<FarmerDto>> GetByEmailAsync(string farmerEmail);
        Task<BaseResponse<IEnumerable<FarmerDto>>> GetAllAsync();
        Task<BaseResponse<ActiveAndNonActiveFarmers>> GetAllActiveAndNonActiveAsync();
         Task<BaseResponse<IEnumerable<FarmerDto>>> SearchFarmerByEmailOrUserName(string searchInput);
        Task<BaseResponse<FarmerDto>> UpdateAsync(UpdateFarmerRequestModel updateFarmerModel, string id);
        Task UpdateToHasPaidDue(string userEmail);
        Task DeleteAsync(string farmerId);
        Task FarmerMonthlyDueUpdate();
    }
