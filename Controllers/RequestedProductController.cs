using System.Security.Claims;
using AgroExpressAPI.Dtos;
using AgroExpressAPI.Dtos.RequestedProduct;
using AgroExpressAPI.Email;
using AgroExpressAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace AgroExpressAPI.Controllers;
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class RequestedProductController : VersionedApiController
{
             private readonly IRequestedProductService _requstedProductService;
             private readonly IProductService _productService;
             private readonly IUserService _userService;
             private readonly ITransactionService _transactionService;
             private readonly IEmailSender _emailSender;
              private readonly IMemoryCache _memoryCache;
             
        public RequestedProductController(IRequestedProductService requstedProductService, IProductService productService, IUserService userService, ITransactionService transactionService, IEmailSender emailSender, IMemoryCache memoryCache)
        {
            _requstedProductService = requstedProductService;
            _productService = productService;
            _userService = userService;
            _transactionService = transactionService;
            _emailSender = emailSender;
            _memoryCache = memoryCache;
        }

       

         [HttpPost("CreateRequestedProduct/{requestId}")]
        public async Task<IActionResult> CreateRequestedProduct([FromForm]CreateRequestedProductRequestModel requestedModel, [FromRoute]string requestId)
        {
             if(!ModelState.IsValid)
            {
                string response1 = "Invalid input,check your input very well";
                return BadRequest(new { message = response1 });
            }
            var mail = User.FindFirst(ClaimTypes.Email).Value;
            requestedModel.BuyerEmail = mail;   
            requestedModel.ProductId = requestId;
           var user = await _userService.GetByEmailAsync(requestedModel.BuyerEmail);
           if(user.Data.Haspaid == false)
           { 
              return BadRequest();
           }
                   //var product =  await  _requstedProductService.CreateRequstedProductAsync(requestId,requestedModel);
                     var product =  await  _transactionService.MakePayment(requestedModel);
                    if(product.IsSuccess != true) return BadRequest(product);
                    return Ok(product);     
         }


        [HttpGet("OrderedProductAndPendingProduct")]
          public async Task<IActionResult> OrderedProductAndPendingProduct(string buyerEmail)
        {
            if(buyerEmail == null) buyerEmail = User.FindFirst(ClaimTypes.Email).Value;
             if (!_memoryCache.TryGetValue($"Ordered_And_Pending_Product_By_{buyerEmail}", out BaseResponse<OrderedRequestAndPendingRequest> results))
            {
                 results =  await  _requstedProductService.OrderedAndPendingProduct(buyerEmail);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
                };
                 _memoryCache.Set($"Ordered_And_Pending_Product_By_{buyerEmail}", results, cacheEntryOptions);

            }
                if(results.IsSuccess != true) return BadRequest(results);
               return Ok(results);
         }

           [HttpGet("GetRequestedProductById/{requestId}")]
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
           return Ok(new { message = response });
            
         }


        [HttpPatch("DeliveredRequest/{requestId}")]
         public async Task<IActionResult> DeliveredRequest([FromRoute]string requestId)
         {
            if(!string.IsNullOrWhiteSpace(requestId))
            {
                await _requstedProductService.ProductDelivered(requestId);
                string response = "Thank You !";
                return Ok(new { message = response });
            }
            return BadRequest();
            
         }
         [HttpPatch("NotDeliveredRequest/{requestId}")]
          public async Task<IActionResult> NotDeliveredRequest([FromRoute]string requestId)
        {
            await _requstedProductService.NotDelivered(requestId);
           string response = "We will get back to you soon!";
           return Ok(new { message = response });

        }


           [HttpPatch("AcceptRequest/{requestId}")]
           public async Task<IActionResult> AcceptRequest([FromRoute]string requestId)
         {
            if(string.IsNullOrWhiteSpace(requestId))
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
             if (!_memoryCache.TryGetValue($"Requst_For_Farmer_With_Id_{farmerId}", out BaseResponse<IEnumerable<RequestedProductDto>> requests))
            {
                 requests =  await _requstedProductService.MyRequests(farmerId); 
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Cache for 10 minutes
                };
                 _memoryCache.Set($"Requst_For_Farmer_With_Id_{farmerId}", requests, cacheEntryOptions);

            }
            if(requests.IsSuccess == false) return BadRequest(requests);
            return Ok(requests);
        }


         [HttpPatch("UpdateToHasPaid/{userEmail}")]
        public IActionResult UpdateToHasPaid([FromRoute]string userEmail)
        {
              _userService.UpdatingToHasPaid(userEmail);
              string response = "Payment successful";
                return Ok(new { message = response });
        }
    }
