using System.Security.Claims;
using AgroExpressAPI.Dtos.Farmer;
using AgroExpressAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroExpressAPI.Controllers;
    [Route("api/[controller]")]
    [ApiController]
    public class FarmerController : ControllerBase
    {
         private readonly IFarmerService _farmerService;
        private readonly IUserService _userService;
         private readonly IWebHostEnvironment _webHostEnvironment;
        public FarmerController(IFarmerService farmerService, IUserService userService,IWebHostEnvironment webHostEnvironment)
        {
            _farmerService = farmerService;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
            
        }

       
        [HttpPost("CreateFarmer")]
        // [ValidateAntiForgeryToken]
         public async Task<IActionResult> CreateFarmer([FromForm]CreateFarmerRequestModel farmerModel)
        {
             if(!ModelState.IsValid)
            {
                string response = "Invalid input,check your input very well";
                return BadRequest(new{mesage = response});
            }
          var farmerExist = await _userService.ExistByEmailAsync(farmerModel.Email);
            if(!(farmerExist))
            {
              //handling the files in coming from the request
                var files = HttpContext.Request.Form;
                if (files != null && files.Count > 0)
                {
                    string imageDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    if(!Directory.Exists(imageDirectory))Directory.CreateDirectory(imageDirectory);
                    foreach (var file in files.Files)
                    {
                        FileInfo info = new FileInfo(file.FileName);
                        var extension = info.Extension;
                        string[] extensions =  new string[]{".png",".jpeg",".jpg",".gif",".tif"};
                        bool check = false;
                        foreach(var ex in extensions)
                        {
                            if(extension == ex) check = true;
                        }
                        if(check == false) return BadRequest(new{mesage ="The type of your profile picture is not accepted"});
                        if(file.Length > 20480) return BadRequest(new{mesage = "accepted profile picture must not be more than 20KB"});
                        string image = Guid.NewGuid().ToString() + info.Extension;
                        string path = Path.Combine(imageDirectory, image);
                        using(var filestream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(filestream);
                        }
                        farmerModel.ProfilePicture = (image);
                    }
                }
                
                        var farmer = await _farmerService.CreateAsync(farmerModel);

                        if(farmer.IsSuccess == false) return BadRequest(farmer);
                        
                  return Ok(farmer);
            }
            string userExist = "user already exist ⚠";
          return BadRequest(new{mesage = userExist});
            
            
        }  
         
        [HttpGet("FarmerProfile/{farmerEmail}")]
          public async Task<IActionResult> FarmerProfile([FromRoute]string farmerEmail)
        {
            if(string.IsNullOrWhiteSpace(farmerEmail)) farmerEmail = User.FindFirst(ClaimTypes.Email).Value;
            var farmer = await _farmerService.GetByEmailAsync(farmerEmail);
            if(farmer.IsSuccess == false) return BadRequest(farmer);
            return Ok(farmer);
        } 
         
       [HttpGet("GetFarmerById/{farmerId}")]
        public async Task<IActionResult> GetFarmerById([FromRoute]string farmerId)
        {       
            if(string.IsNullOrWhiteSpace(farmerId)) farmerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
           var farmer = await _farmerService.GetByIdAsync(farmerId);
             if(farmer.IsSuccess == false) return BadRequest(farmer);
             return Ok(farmer);
        }

         [HttpGet("GetFarmerByEmail/{farmerEmail}")]
        public async Task<IActionResult> GetFarmerByEmail([FromRoute]string farmerEmail)
        {       
            if(string.IsNullOrWhiteSpace(farmerEmail)) farmerEmail = User.FindFirst(ClaimTypes.Email).Value;
           var farmer = await _farmerService.GetByEmailAsync(farmerEmail);
             if(farmer.IsSuccess == false) return BadRequest(farmer);
             return Ok(farmer);
        }
        
         [HttpPut("UpdateFarmer/{id}")]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> UpdateFarmer(UpdateFarmerRequestModel requestModel, string id)
        {
              if(string.IsNullOrWhiteSpace(requestModel.Email)) requestModel.Email = User.FindFirst(ClaimTypes.Email).Value;
              if(string.IsNullOrWhiteSpace(id))id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var farmer = await _farmerService.UpdateAsync(requestModel,id);
             if(farmer.IsSuccess == false) return BadRequest(farmer);
             return Ok(farmer);
        }

          [HttpPatch("DeleteFarmer/{farmerId}")]
         public IActionResult DeleteFarmer([FromRoute]string farmerId)
        {
            if(string.IsNullOrWhiteSpace(farmerId)) farmerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _farmerService.DeleteAsync(farmerId);
            return Ok();
        }
          
        //  [Authorize(Roles = "Admin")]
        [HttpGet("Farmers")]
        public async Task<IActionResult> Farmers()
        {
            var farmers = await _farmerService.GetAllActiveAndNonActiveAsync();
            if(farmers.IsSuccess == false) return BadRequest(farmers);
            return Ok(farmers);

        }
        
           [HttpGet("SearchFarmers/{searchInput}")]
         public async Task<IActionResult> SearchFarmers([FromRoute]string searchInput)
        {
             if(string.IsNullOrWhiteSpace(searchInput)) return BadRequest();
             var farmers = await _farmerService.SearchFarmerByEmailOrUserName(searchInput);
              if(farmers.IsSuccess == false) return BadRequest(farmers);
             return Ok(farmers);
        }

         [HttpPatch("UpdateToHasPaid/{userEmail}")]
        public IActionResult UpdateToHasPaid([FromRoute]string userEmail)
        {
              _farmerService.UpdateToHasPaidDue(userEmail);
              string response = "Due Paid successfully";
                 return Ok(new{mesage = response});
        }

        
    }
