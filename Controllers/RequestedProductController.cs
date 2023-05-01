using System.Security.Claims;
using AgroExpressAPI.Dtos.RequestedProduct;
using AgroExpressAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroExpressAPI.Controllers;
   [Route("api/[controller]")]
   [ApiController]
    public class RequestedProductController : ControllerBase
    {
          private readonly IRequestedProductService _requstedProductService;
           private readonly IProductService _productService;
             private readonly IUserService _userService;
             private readonly ITransactionService _transactionService;
             
        public RequestedProductController(IRequestedProductService requstedProductService, IProductService productService, IUserService userService, ITransactionService transactionService)
        {
            _requstedProductService = requstedProductService;
            _productService = productService;
            _userService = userService;
            _transactionService = transactionService;
        }

       

         [HttpPost("CreateRequestedProduct/{requestId}")]
        public async Task<IActionResult> CreateRequestedProduct([FromForm]CreateRequestedProductRequestModel requestedModel, string requestId)
        {
             if(!ModelState.IsValid)
            {
                string response1 = "Invalid input,check your input very well";
                return BadRequest(response1);
            }
            // var userEmail = User.FindFirst(ClaimTypes.Email).Value;
             var userEmail = requestedModel.BuyerEmail;
           var user = await _userService.GetByEmailAsync(userEmail);
           if(user.Data.Haspaid == true)
           {
                    // var product =  await  _requstedProductService.CreateRequstedProductAsync(requestId,requestedModel);
                     var product =  await  _transactionService.MakePayment(requestedModel);
                    if(product.IsSuccess != true) return BadRequest(product);
                    return Ok(product);  

           }
           string response = "you are yet to pay request token";
            return BadRequest(response);
         }


        [HttpGet("OrderedProductAndPendingProduct")]
          public async Task<IActionResult> OrderedProductAndPendingProduct(string buyerEmail)
        {
                    if(buyerEmail == null) buyerEmail = User.FindFirst(ClaimTypes.Email).Value;
                var results =  await  _requstedProductService.OrderedAndPendingProduct(buyerEmail);
                if(results.IsSuccess != true) return BadRequest(results);
               return Ok(results);
         }

           [HttpGet("GetRequestedProductById/{productId}")]
        public async Task<IActionResult> GetRequestedProductById([FromRoute]string requestId)
        {       
            if(string.IsNullOrWhiteSpace(requestId)) return BadRequest();
           var product = await _requstedProductService.GetRequestedProductById(requestId);
             if(product.IsSuccess == false) return BadRequest(product);
             return Ok(product);
        }

        [HttpDelete("DeleteRequest/{requestId}")]
         public async Task<IActionResult> DeleteRequest([FromRoute]string requestId)
         {
            if(string.IsNullOrWhiteSpace(requestId)) return BadRequest();
           await _requstedProductService.DeleteRequestedProduct(requestId);
           string response = "Requested Product Deleted successfully";
           return Ok(response);
            

         }


        [HttpPatch("DeliveredRequest/{requestId}")]
         public async Task<IActionResult> DeliveredRequest([FromRoute]string requestId)
         {
            if(requestId != null)
            {
                await _requstedProductService.ProductDelivered(requestId);
                string response = "Thank You !";
                return Ok(response);
            }
            return BadRequest();
            
         }
         [HttpPatch("NotDeliveredRequest/{requestId}")]
          public async Task<IActionResult> NotDeliveredRequest([FromRoute]string requestId)
        {
            await _requstedProductService.NotDelivered(requestId);
           string response = "We will get back to you soon!";
           return Ok(response);

        }


           [HttpPatch("AcceptRequest/{requestId}")]
           public async Task<IActionResult> AcceptRequest([FromRoute]string requestId)
         {
            if(requestId != null)
            {
                var product = await _requstedProductService.ProductAccepted(requestId);
                Ok(product);
            }
           return BadRequest();
         }

         [HttpGet("MyRequests/{farmerId}")]
        public async Task<IActionResult> MyRequests([FromRoute]string farmerId)
        {
            farmerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
           var requests =  await _requstedProductService.MyRequests(farmerId); 
            if(requests.IsSuccess == false) return BadRequest(requests);
            return Ok(requests);
        }


         [HttpPatch("UpdateToHasPaid/{userEmail}")]
        public IActionResult UpdateToHasPaid([FromRoute]string userEmail)
        {
              _userService.UpdatingToHasPaid(userEmail);
              string response = "Payment successful";
                return Ok(response);
        }
    }
