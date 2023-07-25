using AgroExpressAPI.Dtos;
using AgroExpressAPI.Dtos.User;
using System.Security.Claims;

namespace AgroExpressAPI.ApplicationAuthentication;
    public interface IJWTAuthentication
    {
         string GenerateToken(UserDto model);
         string GenerateRefreshToken();
         ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
