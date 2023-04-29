using AgroExpressAPI.Dtos;
using AgroExpressAPI.Dtos.User;

namespace AgroExpressAPI.Services.Interfaces;
    public interface IUserService
    {
         Task<BaseResponse<UserDto>> GetByIdAsync(string userId);
        Task<BaseResponse<UserDto>> GetByEmailAsync(string userEmail);
        Task<BaseResponse<UserDto>> Login(LogInRequestModel logInRequestModel);
        Task<BaseResponse<IEnumerable<UserDto>>> GetAllAsync();
         Task<BaseResponse<IEnumerable<UserDto>>> SearchUserByEmailOrUserName(string searchInput);
         Task<BaseResponse<IEnumerable<UserDto>>> PendingRegistration();
        BaseResponse<UserDto> UpdateAsync(UpdateUserRequestModel updateUserModel, string id);
        Task DeleteAsync(string userId);
        BaseResponse<UserDto> VerifyUser(string userEmail);
        Task<bool> ExistByEmailAsync(string userEmail);
        Task UpdatingToHasPaid(string email);
        Task<bool> ForgottenPassword(string email);
    }
