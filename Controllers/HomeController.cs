using AgroExpressAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroExpressAPI.Controllers;
   
    public class HomeController : VersionedApiController
{
        private readonly IProductService _productSercice;
        private readonly IFarmerService _farmerSercice;
        private readonly ILogger _logger;

        public HomeController(IProductService productSercice, IFarmerService farmerSercice,ILoggerFactory logger)
        {
            _productSercice = productSercice;
            _farmerSercice = farmerSercice;
             _logger = logger.CreateLogger("MyCategory");
        
        }  

         [HttpGet("Index")]
         public async Task<IActionResult> Index()
        {
            await _productSercice.DeleteExpiredProducts();
            await _farmerSercice.FarmerMonthlyDueUpdate();
            var products = await _productSercice.GetAllFarmProductAsync();
        _logger.LogInformation("Get pages.ContentModel called");
            if(products.IsSuccess == false) return BadRequest(products);
                return Ok(products);
        }
    }
