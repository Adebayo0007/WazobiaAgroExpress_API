using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AgroExpressAPI.Dtos;
using AgroExpressAPI.Dtos.User;
using Microsoft.IdentityModel.Tokens;

namespace AgroExpressAPI.ApplicationAuthentication;
    public class JWTAuthentication : IJWTAuthentication
    {
        public string _key;
        public JWTAuthentication(string key)
        {
            _key = key;
        }
        public string GenerateToken(BaseResponse<UserDto> model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);
             var claims = new List<Claim>();
             claims.Add(new Claim(ClaimTypes.NameIdentifier, model.Data.Id));                    
             claims.Add(new Claim(ClaimTypes.Email, model.Data.Email));
            //  foreach (var role in model.Roles)
            // {
                claims.Add(new Claim(ClaimTypes.Role, model.Data.Role));
            //}         
                                 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                IssuedAt= DateTime.Now,
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature),
                    // Issuer = "",
                    // Audience = ""
            };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
        }
    }
