using AgroExpressAPI.Dtos.Product;
using AgroExpressAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroExpressAPI.Controllers;
   [Route("api/[controller]")]
   [ApiController]
    public class ProductController : ControllerBase
    {
         private readonly IProductService _productService;
        public ProductController(IProductService productSercice)
        {
            _productService = productSercice;   
        }
       
         [HttpPost("CreateProduct")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct([FromForm]CreateProductRequestModel model)
        {
             if(!ModelState.IsValid)
            {
                string response = "Invalid input,check your input very well";
                return BadRequest(response);
            }   
            var product = await _productService.CreateProductAsync(model);
            if(product.IsSuccess == false) return BadRequest(product);
            return Ok(product);
        }


         [HttpGet("MyProducts")]
        public async Task<IActionResult> MyProducts()
        {
           var products =  await _productService.GetFarmerFarmProductsByIdAsync();
            if(products.IsSuccess == false) return BadRequest(products);
            return Ok(products);
        }


        [HttpGet("AvailableProducts")]
           public async Task<IActionResult> AvailableProducts()
        {
           var products =  await _productService.GetAllFarmProductByLocationAsync();
            if(products.IsSuccess == false) return BadRequest(products);
            return Ok(products);
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
            return Ok(response);
        }


          [HttpGet("SearchProduct/{searchInput}")]
         public async Task<IActionResult> SearchProduct([FromRoute]string searchInput)
        {
            if(string.IsNullOrWhiteSpace(searchInput)) return BadRequest();
        
             var products = await _productService.SearchProductsByProductNameOrFarmerUserNameOrFarmerEmail(searchInput);
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
             return Ok(response);
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
             return Ok(response);
            }
              return BadRequest();
        }  
    }
