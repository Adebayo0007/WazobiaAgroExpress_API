using AgroExpressAPI.Dtos.Product;
using AgroExpressAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroExpressAPI.Controllers;
   [Route("api/[controller]")]
   [ApiController]
    public class ProductController : ControllerBase
    {
         private readonly IProductService _productService;
         private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductService productSercice, IWebHostEnvironment webHostEnvironment)
        {
            _productService = productSercice;  
            _webHostEnvironment = webHostEnvironment; 
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
                        if(check == false) return BadRequest("The type of your picture is not accepted");
                        if(file.Length > 20480) return BadRequest("accepted picture must not be more than 20KB");
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
