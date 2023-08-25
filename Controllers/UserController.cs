using System.Security.Claims;
using AgroExpressAPI.ApplicationAuthentication;
using AgroExpressAPI.Dtos.User;
using AgroExpressAPI.Dtos;
using AgroExpressAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AgroExpressAPI.Controllers;

    //[Route("api/v{version:apiVersion}/[controller]")]
    //[ApiController]
      [ApiVersion("1.0")]
      [ApiVersion("2.0")]
public class UserController : VersionedApiController
{

         private readonly IUserService _userService;
         private readonly IJWTAuthentication _authentication;
          private readonly IMemoryCache _memoryCache;
        public UserController(IUserService userService, IJWTAuthentication authentication, IMemoryCache memoryCache)
        {
            _userService = userService;
            _authentication = authentication;
            _memoryCache = memoryCache;
            
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
             if (!_memoryCache.TryGetValue($"Application_Users", out BaseResponse<IEnumerable<UserDto>> users))
            {
                 users =  await _userService.GetAllAsync();
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
                };
                 _memoryCache.Set($"Application_Users", users, cacheEntryOptions);

            }
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

              if (!_memoryCache.TryGetValue($"Searched_Users_{searchInput}", out BaseResponse<IEnumerable<UserDto>> users))
            {
                 users =  await _userService.SearchUserByEmailOrUserName(searchInput);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
                };
                 _memoryCache.Set($"Searched_Users_{searchInput}", users, cacheEntryOptions);
            }

              if(users.IsSuccess == false)
            {
                return BadRequest(users);
            }
             return Ok(users);
        }
          
          [HttpGet("PendingRegistration")]
        public async Task<IActionResult> PendingRegistration()
        {
              if (!_memoryCache.TryGetValue($"Pending_Registration", out BaseResponse<IEnumerable<UserDto>> pendingRequests))
            {
                 pendingRequests =  await _userService.PendingRegistration();
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
                };
                 _memoryCache.Set($"Pending_Registration", pendingRequests, cacheEntryOptions);
            }
            
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
