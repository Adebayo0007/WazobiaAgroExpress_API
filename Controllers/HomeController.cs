using AgroExpressAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgroExpressAPI.Controllers;
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IProductService _productSercice;
        private readonly IFarmerService _farmerSercice;

        public HomeController(IProductService productSercice, IFarmerService farmerSercice)
        {
            _productSercice = productSercice;
            _farmerSercice = farmerSercice;
        }  

         [HttpGet("Index")]
         public async Task<IActionResult> Index()
        {
            await _productSercice.DeleteExpiredProducts();
            await _farmerSercice.FarmerMonthlyDueUpdate();
            var products = await _productSercice.GetAllFarmProductAsync();
            if(products.IsSuccess == false) return BadRequest(products);
                return Ok(products);
        }
    }
