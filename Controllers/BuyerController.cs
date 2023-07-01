using System.Security.Claims;
using AgroExpressAPI.Dtos.Buyer;
using AgroExpressAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroExpressAPI.Controllers;
   [Route("api/[controller]")]
    [ApiController]
    public class BuyerController : ControllerBase
    {
         private readonly IBuyerService _buyerService;
           private readonly IUserService _userService;
            private readonly IWebHostEnvironment _webHostEnvironment;
        public BuyerController(IBuyerService buyerService, IUserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _buyerService = buyerService;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
            
        }

         [HttpPost("CreateBuyer")]
         //[ValidateAntiForgeryToken]
         public async Task<IActionResult> CreateBuyer([FromForm]CreateBuyerRequestModel buyerModel)
        {
            if(!ModelState.IsValid)
            {
                string response = "Invalid input,check your input very well";
                return BadRequest(new{mesage = response});
            }
            if(buyerModel.LocalGovernment == "--LGA--")
            {
                 string response = "Invalid Local Government";
                return BadRequest(new{mesage = response});
            }

            var buyerExist = await _userService.ExistByEmailAsync(buyerModel.Email);
            if(!(buyerExist))
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
                    foreach(var ext in extensions)
                    {
                        if(extension.Equals(ext)) check = true;
                    }
                    if(check == false) return BadRequest(new{mesage = "The type of your profile picture is not accepted"} );
                    if(file.Length > 20480) return BadRequest(new{mesage = "accepted profile picture must not be more than 20KB"});
                    string image = Guid.NewGuid().ToString() + info.Extension;
                    string path = Path.Combine(imageDirectory, image);
                    using(var filestream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    buyerModel.ProfilePicture = (image);
                }
            }
        
                     
                      var buyer = await _buyerService.CreateAsync(buyerModel);

                        if(buyer.IsSuccess == false)
                        {
                            return BadRequest(buyer);
                        }

                  return Ok(buyer);
            }
            string userExist = "user already exist ⚠";
          return BadRequest(new{mesage = userExist});
            
        } 


         [HttpGet("BuyerProfile/{buyerEmail}")]
          public async Task<IActionResult> BuyerProfile([FromRoute]string buyerEmail)
        {
            if(string.IsNullOrWhiteSpace(buyerEmail)) buyerEmail = User.FindFirst(ClaimTypes.Email).Value;
            var buyer = await _buyerService.GetByEmailAsync(buyerEmail);
            if(buyer.IsSuccess == false) return BadRequest(buyer);
            return Ok(buyer);
        } 


           [HttpGet("GetBuyerById/{buyerId}")]
        public async Task<IActionResult> GetBuyerById([FromRoute]string buyerId)
        {       
            if(string.IsNullOrWhiteSpace(buyerId)) buyerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
           var buyer = await _buyerService.GetByIdAsync(buyerId);
             if(buyer.IsSuccess == false)
            {
                return BadRequest(buyer);
            }
             return Ok(buyer);
        }

         [HttpGet("GetBuyerByEmail/{buyerEmail}")]
        public async Task<IActionResult> GetBuyerByEmail([FromRoute]string buyerEmail)
        {       
            if(string.IsNullOrWhiteSpace(buyerEmail)) buyerEmail = User.FindFirst(ClaimTypes.Email).Value;
           var buyer = await _buyerService.GetByEmailAsync(buyerEmail);
             if(buyer.IsSuccess == false)
            {
                return BadRequest(buyer);
            }
             return Ok(buyer);
        }


          [HttpPut("UpdateBuyer/{id}")]
        //   [ValidateAntiForgeryToken]
         public async Task<IActionResult> UpdateBuyer(UpdateBuyerRequestModel requestModel,string id)
        {
              if(string.IsNullOrWhiteSpace(requestModel.Email)) requestModel.Email = User.FindFirst(ClaimTypes.Email).Value;
            
              if(string.IsNullOrWhiteSpace(id)) id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var buyer = await _buyerService.UpdateAsync(requestModel,id);
             if(buyer.IsSuccess == false)
            {
                return BadRequest(buyer);
            }
             return Ok(buyer);
        }


        
        [HttpDelete("DeleteBuyer/{buyerId}")]
         public IActionResult DeleteBuyer([FromRoute]string buyerId)
        {
            if(string.IsNullOrWhiteSpace(buyerId)) buyerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _buyerService.DeleteAsync(buyerId);
            return Ok();
        }
          [Authorize(Roles = "Admin")]
          [HttpGet("Buyers")]
        public async Task<IActionResult> Buyers()
        {
            var buyers = await _buyerService.GetAllActiveAndNonActiveAsync();
            if(buyers.IsSuccess == false) return BadRequest(buyers);
            return Ok(buyers);

        }


         [HttpGet("SearchBuyers/{searchInput}")]
         public async Task<IActionResult> SearchBuyers([FromRoute]string searchInput)
        {
             if(string.IsNullOrWhiteSpace(searchInput)) return BadRequest();
             var buyers = await _buyerService.SearchBuyerByEmailOrUserName(searchInput);
              if(buyers.IsSuccess == false) return BadRequest(buyers);
             return Ok(buyers);
        }
    }
