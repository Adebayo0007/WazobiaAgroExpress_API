using AgroExpressAPI.Dtos;
using AgroExpressAPI.Dtos.User;

namespace AgroExpressAPI.ApplicationAuthentication;
    public interface IJWTAuthentication
    {
         string GenerateToken(BaseResponse<UserDto> model);
    }
