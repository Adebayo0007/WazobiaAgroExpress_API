using System.Security.Claims;
using AgroExpressAPI.ApplicationAuthentication;
using AgroExpressAPI.Dtos.User;
using AgroExpressAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroExpressAPI.Controllers;

    //[Route("api/v{version:apiVersion}/[controller]")]
    //[ApiController]
      [ApiVersion("1.0")]
      [ApiVersion("2.0")]
public class UserController : VersionedApiController
{

         private readonly IUserService _userService;
         private readonly IJWTAuthentication _authentication;
        public UserController(IUserService userService, IJWTAuthentication authentication)
        {
            _userService = userService;
            _authentication = authentication;
            
        }
       /* [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]*/
        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn(LogInRequestModel loginModel)
        {
             if(!ModelState.IsValid)
            {
                string response1 = "Invalid input,check your input very well";
                return BadRequest(response1);
            }
            if (string.IsNullOrWhiteSpace(loginModel.Email)  || string.IsNullOrWhiteSpace(loginModel.Password))
            {
                return NotFound();
            }
            var user = await _userService.Login(loginModel);
            if (user.Data is null)
            {
                return Unauthorized();
            }
        if (!user.IsSuccess)
               {
                   return BadRequest(user);
               }

            var token = _authentication.GenerateToken(user.Data);
             var refreshToken = _authentication.GenerateRefreshToken();
             await _userService.UpdateRefreshToken(user.Data.Email, refreshToken);
               var response = new LogInResponseModel<UserDto>
               {
                   Data = user.Data,
                   Token = token,
                   RefreshToken = refreshToken,
                   IsSuccess = true
               };
               return Ok(response);
              

        }

       [Authorize(Roles ="Admin")]
       [HttpGet("ApplicationUsers")]
        public async Task<IActionResult> ApplicationUsers()
        {
            var users = await _userService.GetAllAsync();
            if(users.IsSuccess == false)
            {
               return BadRequest(users);
            }
            return Ok(users);

        }

         [HttpGet("GetUserById/{userId}")]
        public async Task<IActionResult> GetUserById([FromRoute]string userId)
        {       
             if(string.IsNullOrWhiteSpace(userId)) userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
           var user = await _userService.GetByIdAsync (userId);
             if(user.IsSuccess == false)
            {
                return BadRequest(user);
            }
             return Ok(user);
        }

         [HttpGet("GetUserByEmail/{userEmail}")]
        public async Task<IActionResult> GetUserByEmail([FromRoute]string userEmail)
        {       
            if(string.IsNullOrWhiteSpace(userEmail)) userEmail = User.FindFirst(ClaimTypes.Email).Value;
           var user = await _userService.GetByEmailAsync(userEmail);
             if(user.IsSuccess == false)
            {
                return BadRequest(user);
            }
             return Ok(user);
        }
        
        [HttpPatch("DeleteUser/{userId}")]
         public async Task<IActionResult> DeleteUser([FromRoute]string userId)
        {
            if(string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest();
            }
            await _userService.DeleteAsync(userId);
            string response = "user deleted successfully";
           return Ok(response);
        }
           
         [HttpGet("SearchUser/{searchInput}")]
         public async Task<IActionResult> SearchUser([FromRoute]string searchInput)
        {
             if(string.IsNullOrWhiteSpace(searchInput))
            {
                 return BadRequest();
            }

             var users = await _userService.SearchUserByEmailOrUserName(searchInput);
              if(users.IsSuccess == false)
            {
                return BadRequest(users);
            }
             return Ok(users);
        }
          
          [HttpGet("PendingRegistration")]
        public async Task<IActionResult> PendingRegistration()
        {
            var pendingRequests = await _userService.PendingRegistration();
              if(pendingRequests.IsSuccess == false)
            {
                return BadRequest(pendingRequests);
            }
             return Ok(pendingRequests);
        }
        
         [HttpPatch("VerifyUser/{userEmail}")]
        public IActionResult VerifyUser([FromRoute]string userEmail)
        {
            if(!(string.IsNullOrWhiteSpace(userEmail)))
            {
                var user = _userService.VerifyUser(userEmail);
                return Ok(user);
            }
            return BadRequest();
        }

          
        [HttpPost("ForgottenPassword")]
         public async Task<IActionResult> ForgottenPassword(string userEmail)
        {
            var response = await _userService.ForgottenPassword(userEmail);
            if(response == false)
            {
                return BadRequest(response);
            }
             return Ok(response);
        }
    }
