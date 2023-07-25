using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AgroExpressAPI.Dtos;
using AgroExpressAPI.Dtos.User;
using Microsoft.IdentityModel.Tokens;

namespace AgroExpressAPI.ApplicationAuthentication;
public class JWTAuthentication : IJWTAuthentication
{
        public string _key = "Wazobia Authorization key is used in authenticating application users into the application and likewise its it used in authorizing each of the users for proper management and security purpose";
       // public IConfiguration _configuration;
        public JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        // public JWTAuthentication(IConfiguration configuration = null)
        // {          
        //     _configuration = configuration;
        //       _key = _configuration.GetValue<string>("JWTConnectionKey:JWTKey");
        // }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using(var rng = RandomNumberGenerator.Create()) 
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public string GenerateToken(UserDto model)
        {
             
            var tokenKey = Encoding.UTF8.GetBytes(_key);
             var claims = new List<Claim>();
             claims.Add(new Claim(ClaimTypes.NameIdentifier, model.Id));                    
             claims.Add(new Claim(ClaimTypes.Email, model.Email));
             // claims.Add(new Claim(ClaimTypes.Actor, model.Name));
            //  foreach (var role in model.Roles)
            // {
                claims.Add(new Claim(ClaimTypes.Role, model.Role));
            //}         
                                 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                IssuedAt= DateTime.Now,
                Expires = DateTime.Now.AddMinutes(5),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature),
               
            };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
        }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,//you might want to validate audience and issuer depending on the use cases
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
            ValidateLifetime = false //this means we dont care about token's expiration date
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
            var pricipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid Token");
        return pricipal;
    }

    public void TokenValidatorHandler(string tokenInput)
        {
            var key = _key;
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var principal = tokenHandler.ValidateToken(tokenInput, tokenValidationParameters, out var validatedToken);
        }
    }
