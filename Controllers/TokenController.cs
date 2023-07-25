
using AgroExpressAPI.ApplicationAuthentication;
using AgroExpressAPI.Dtos.User;
using AgroExpressAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroExpressAPI.Controllers;
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        
         private readonly IUserService _userService;
          private readonly IJWTAuthentication _authentication;
          public TokenController(IUserService userService, IJWTAuthentication authentication)
          {
            _userService = userService;
            _authentication = authentication;
          }
          [HttpPost]
          [Route("Refresh")]
          public async Task<IActionResult> Refresh(TokenApiModel model)
          {
            if(model is null) return BadRequest("Invalid client request");
            string accessToken = model.AccessToken;
            string refreshToken = model.RefreshToken;

            var principal = _authentication.GetPrincipalFromExpiredToken(accessToken);
            var name = principal.Identity.Name; //This is mapped to the name claim by default
            //var name = User.FindFirst(ClaimTypes.Actor).Value;
                var userResponse = await _userService.GetByName(name);
                if(userResponse.Data is null 
                || userResponse.Data.RefreshToken != refreshToken 
                || userResponse.Data.RefreshTokenExpiryTime <= DateTime.Now)
            return BadRequest("Invalid Client request");
            var newAccessToken = _authentication.GenerateToken(userResponse.Data);
            var newRefreshToken = _authentication.GenerateRefreshToken();

            await _userService.UpdateRefreshToken(userResponse.Data.Email, newRefreshToken);

             var response = new LogInResponseModel<UserDto>
               {
                   Data = userResponse.Data,
                   Token = newAccessToken,
                   RefreshToken = newRefreshToken,
                   IsSuccess = true
               };
               return Ok(response);
          }

        [HttpPost, Authorize]
        [Route("Revoke")]
          public async Task<IActionResult> Revoke()
          {
            var userName = User.Identity.Name;
            var userResponse = await _userService.GetByName(userName);
            if(userResponse.Data is null) return BadRequest();
            await _userService.UpdateRefreshToken(userResponse.Data.Email, null);
            return NoContent();

          }
    }