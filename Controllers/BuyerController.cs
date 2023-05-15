using System.Security.Claims;
using AgroExpressAPI.Conversion;
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
        public BuyerController(IBuyerService buyerService, IUserService userService,IWebHostEnvironment webHostEnvironment)
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
                return BadRequest(response);
            }
            if(buyerModel.LocalGovernment == "--LGA--")
            {
                 string response = "Invalid Local Government";
                return BadRequest(response);
            }

            var buyerExist = await _userService.ExistByEmailAsync(buyerModel.Email);
            if(!(buyerExist))
            {

                        var buyer = await _buyerService.CreateAsync(buyerModel);

                        if(buyer.IsSuccess == false)
                        {
                            return BadRequest(buyer);
                        }

                  return Ok(buyer);
            }
            string userExist = "user already exist ⚠";
          return BadRequest(userExist);
            
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
          [ValidateAntiForgeryToken]
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


         [HttpPost("SearchBuyers")]
         public async Task<IActionResult> SearchBuyers(string searchInput)
        {
             if(string.IsNullOrWhiteSpace(searchInput)) return BadRequest();
             var buyers = await _buyerService.SearchBuyerByEmailOrUserName(searchInput);
              if(buyers.IsSuccess == false) return BadRequest(buyers);
             return Ok(buyers);
        }
    }
