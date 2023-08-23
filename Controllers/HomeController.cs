using AgroExpressAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroExpressAPI.Controllers;
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class HomeController : VersionedApiController
{
        private readonly IProductService _productSercice;
        private readonly IFarmerService _farmerSercice;
    private readonly ILogger<HomeController> _logger;

        public HomeController(IProductService productSercice, IFarmerService farmerSercice, ILogger<HomeController> logger)
        {
            _productSercice = productSercice;
            _farmerSercice = farmerSercice;
             _logger = logger;
        
        }  

         [HttpGet("Index")]
         public async Task<IActionResult> Index()
        {
            await _productSercice.DeleteExpiredProducts();
            await _farmerSercice.FarmerMonthlyDueUpdate();
            var products = await _productSercice.GetAllFarmProductAsync();
            _logger.LogInformation("Get pages.ContentModel called");
          _logger.Log(LogLevel.Information, "This is logging from the home class", new {name = products});
         _logger.LogInformation("This is logging from the home class", new {name = products});
        if (products.IsSuccess == false) return BadRequest(products);
                return Ok(products);
        }
    }
