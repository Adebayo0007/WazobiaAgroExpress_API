using AgroExpressAPI.Dtos.Product;
using AgroExpressAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AgroExpressAPI.Dtos;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace AgroExpressAPI.Controllers;
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class ProductController : VersionedApiController
{
         private readonly IProductService _productService;
         private readonly IWebHostEnvironment _webHostEnvironment;
          private readonly IMemoryCache _memoryCache;
        public ProductController(IProductService productSercice, IWebHostEnvironment webHostEnvironment, IMemoryCache memoryCache)
        {
            _productService = productSercice;  
            _webHostEnvironment = webHostEnvironment; 
             _memoryCache = memoryCache;
        }
       
         [HttpPost("CreateProduct")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct([FromForm]CreateProductRequestModel model)
        {
             if(!ModelState.IsValid)
            {
                string response = "Invalid input,check your input very well";
                return BadRequest(new { message = response });
            }   
                  //handling the files in coming from the request
                   IList<string> dimentions =  new List<string>();
                   var files = HttpContext.Request.Form;
                if (files != null && files.Count > 0)
                {
                    string imageDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "products");
                    if(!Directory.Exists(imageDirectory))Directory.CreateDirectory(imageDirectory);
                    foreach (var file in files.Files)
                    {
                        FileInfo info = new FileInfo(file.FileName);
                        var extension = info.Extension;
                        string[] extensions =  new string[]{".png",".jpeg",".jpg",".gif",".tif"};
                        bool check = false;
                        foreach(var ext in extensions)
                        {
                            if(extension == ext) check = true;
                        }
                        if(check == false) return BadRequest(new { message = "The type of your picture is not accepted" });
                        if(file.Length > 20480) return BadRequest(new { message = "accepted picture must not be more than 20KB" });
                        string image = Guid.NewGuid().ToString() + info.Extension;
                        string path = Path.Combine(imageDirectory, image);
                        using(var filestream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(filestream);
                        }
                        dimentions.Add(image);
                         
                    }
                  
                }
            
                      for(int x = 0; x < 4; x++)
                         {
                          if(x == 0)model.FirstDimentionPicture = dimentions[x];
                          if(x == 1)model.SecondDimentionPicture = dimentions[x];
                          if(x == 2)model.ThirdDimentionPicture = dimentions[x] ;
                          if(x == 3)model.ForthDimentionPicture = dimentions[x] ;
                         }
            var product = await _productService.CreateProductAsync(model);
            if(product.IsSuccess == false) return BadRequest(product);
            return Ok(product);
        }


         [HttpGet("MyProducts")]
         [ResponseCache(Duration = 3600,Location = ResponseCacheLocation.Any)]  //using cache as an attribute for fast 
        public async Task<IActionResult> MyProducts()
        {
          var email = User.FindFirst(ClaimTypes.Email).Value;
            if (!_memoryCache.TryGetValue($"FarmerProducts_With_Email_{email}", out BaseResponse<IEnumerable<ProductDto>> products))
            {
                 products =  await _productService.GetFarmerFarmProductsByIdAsync(email);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
                };
                 _memoryCache.Set($"FarmerProducts_With_Email_{email}", products, cacheEntryOptions);

            }
          // var products =  await _productService.GetFarmerFarmProductsByIdAsync();
            if(products.IsSuccess == false) return BadRequest(products);
            return Ok(products);
        }


        [HttpGet("AvailableProducts")]
        [ResponseCache(Duration = 3600,Location = ResponseCacheLocation.Any)]  
           public async Task<IActionResult> AvailableProducts()
        {
               if (!_memoryCache.TryGetValue($"Available_Products", out BaseResponse<IEnumerable<ProductDto>> cachedValue))
            {
                 cachedValue =  await _productService.GetAllFarmProductByLocationAsync();
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
                };
                 _memoryCache.Set($"Available_Products", cachedValue, cacheEntryOptions);

            }
              
                    if(cachedValue.IsSuccess == false) return BadRequest(cachedValue);
           
                 return Ok(cachedValue);
        }

         [HttpGet("GetProductById/{productId}")]
        public async Task<IActionResult> GetProductById([FromRoute]string productId)
        {       
            if(string.IsNullOrWhiteSpace(productId)) return BadRequest();
           var product = await _productService.GetProductById(productId);
             if(product.IsSuccess == false) return BadRequest(product);
             return Ok(product);
        }

        [HttpPatch("UpdateProduct/{productsId}")]
        //  [ValidateAntiForgeryToken]
         public async Task<IActionResult> UpdateProduct([FromForm]UpdateProductRequestModel requestModel,[FromRoute]string productsId)
        {
             var product = await _productService.UpdateProduct(requestModel,productsId);
              if(product.IsSuccess == false) return BadRequest(product);
            return Ok(product);
        }
       
            
         [HttpDelete("DeleteProduct/{productId}")]
          public async Task<IActionResult> DeleteProduct([FromRoute]string productId)
        {       
           await _productService.DeleteProduct(productId);
             string response = "product deleted successfully";
            return Ok(new { message = response });
        }


          [HttpGet("SearchProduct/{searchInput}")]
         public async Task<IActionResult> SearchProduct([FromRoute]string searchInput)
        {
            if(string.IsNullOrWhiteSpace(searchInput)) return BadRequest();
             if (!_memoryCache.TryGetValue($"Searched_Product_{searchInput}", out BaseResponse<IEnumerable<ProductDto>> products))
            {
                 products =  await _productService.SearchProductsByProductNameOrFarmerUserNameOrFarmerEmail(searchInput);
                  var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
                };
                 _memoryCache.Set($"Searched_Product_{searchInput}", products, cacheEntryOptions);

            }
            
              if(products.IsSuccess == false) return BadRequest(products);
            return Ok(products);
        }
         
         [HttpPatch("ThumbUp/{productId}")]
        public IActionResult ThumbUp([FromRoute]string productId)
        {
            if(productId != null)
            {
              _productService.ThumbUp(productId);
             var response = "Liked 👍";
             return Ok(new { message = response });
            }
              return BadRequest();
        }

         [HttpPatch("ThumbDown/{productId}")]
          public IActionResult ThumbDown(string productId)
        {
            if(productId != null)
            {
              _productService.ThumbDown(productId);
             var response = "Unlike 👎";
             return Ok(new { message = response });
            }
              return BadRequest();
        }  
    }
