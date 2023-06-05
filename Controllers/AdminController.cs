using System.Security.Claims;
using AgroExpressAPI.Dtos.Admin;
using AgroExpressAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroExpressAPI.Controllers;
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
         private readonly IAdminService _adminService;
          private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(IAdminService adminService,IWebHostEnvironment webHostEnvironment)
        {
            _adminService = adminService;
            _webHostEnvironment = webHostEnvironment;
        }

          [HttpGet("AdminProfile/{adminEmail}")]
         public async Task<IActionResult> AdminProfile([FromRoute]string adminEmail)
        {
            if(string.IsNullOrWhiteSpace(adminEmail)) adminEmail = User.FindFirst(ClaimTypes.Email).Value;
            var admin = await _adminService.GetByEmailAsync(adminEmail);
            if(admin.IsSuccess == false) return BadRequest(admin);
            return Ok(admin);
        }

       
          [HttpGet("GetAdminById/{adminId}")]
        public async Task<IActionResult> GetAdminById([FromRoute]string adminId)
        {       
            if(string.IsNullOrWhiteSpace(adminId)) adminId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
           var admin = await _adminService.GetByIdAsync(adminId);
             if(admin.IsSuccess == false) return BadRequest(admin);
            
             return Ok(admin);
        }
        
        
         [HttpGet("GetAdminByEmail/{adminEmail}")]
        public async Task<IActionResult> GetAdminByEmail([FromRoute]string adminEmail)
        {       
            if(string.IsNullOrWhiteSpace(adminEmail)) adminEmail = User.FindFirst(ClaimTypes.Email).Value;
           var admin = await _adminService.GetByEmailAsync(adminEmail);
             if(admin.IsSuccess == false) return BadRequest(admin);
             return Ok(admin);
        }

          [HttpPut("UpdateAdmin/{id}")]
         //[ValidateAntiForgeryToken]
         public async Task<IActionResult> UpdateAdmin(UpdateAdminRequestModel requestModel,string id)
        {
             if(!ModelState.IsValid)
            {
                string response = "Invalid input,check your input very well";
                return BadRequest(new{mesage = response});
            }

            if(string.IsNullOrWhiteSpace(requestModel.Email))
            {
                requestModel.Email = User.FindFirst(ClaimTypes.Email).Value;
            }
            if(string.IsNullOrWhiteSpace(id)) id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var admin = await _adminService.UpdateAsync(requestModel,id);
             if(admin.IsSuccess == false)
            {
                return BadRequest(admin);
            }
             return Ok(admin);
        }

        [HttpDelete("DeleteAdmin/{adminId}")]
         public IActionResult DeleteAdmin([FromRoute]string adminId)
        {
            if(string.IsNullOrWhiteSpace(adminId)) adminId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _adminService.DeleteAsync(adminId);
            return Ok();
        }
        
    }
