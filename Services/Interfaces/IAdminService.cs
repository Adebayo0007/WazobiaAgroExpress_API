using AgroExpressAPI.Dtos;
using AgroExpressAPI.Dtos.Admin;

namespace AgroExpressAPI.Services.Interfaces;
    public interface IAdminService
    {
         Task<BaseResponse<AdminDto>> GetByIdAsync(string userId);
        Task<BaseResponse<AdminDto>> GetByEmailAsync(string userEmail);
        Task<BaseResponse<IEnumerable<AdminDto>>> GetAllAsync();
        Task<BaseResponse<AdminDto>> UpdateAsync(UpdateAdminRequestModel updateAdminModel, string id);
        Task DeleteAsync(string adminId);
    }
