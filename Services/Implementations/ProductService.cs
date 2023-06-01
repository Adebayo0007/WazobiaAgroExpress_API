using System.Security.Claims;
using AgroExpressAPI.Conversion;
using AgroExpressAPI.Dtos;
using AgroExpressAPI.Dtos.Product;
using AgroExpressAPI.Email;
using AgroExpressAPI.Entities;
using AgroExpressAPI.Repositories.Interfaces;
using AgroExpressAPI.Services.Interfaces;

namespace AgroExpressAPI.Services.Implementations;
public class ProductService : IProductService
{
     private readonly IProductRepository _productRepository;
        private readonly IFarmerRepository _farmerRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
          private readonly IEmailSender _emailSender;

        public ProductService(IProductRepository productRepository, IHttpContextAccessor httpContextAccessor, IFarmerRepository farmerRepository, IUserRepository userRepository, IEmailSender emailSender)
        {
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
            _farmerRepository = farmerRepository;
            _userRepository = userRepository;
            _emailSender = emailSender;
        }
        public async Task<BaseResponse<ProductDto>> CreateProductAsync(CreateProductRequestModel productModel)
        {
             if(productModel.AvailabilityDateFrom.Date > DateTime.Now.Date.AddDays(3))
            {
                 var date = DateTime.Now.Date.AddDays(3).ToString("MM/dd/yyyy");
                 return new BaseResponse<ProductDto>{
                    Message = $"\'Product Availability\' can start from now till  {date} ⚠ ",
                    IsSuccess = false
                };
            }
            if(productModel.AvailabilityDateFrom.Date.Day < DateTime.Now.Date.Day || productModel.AvailabilityDateTo.Date.Day < DateTime.Now.Date.Day || productModel.AvailabilityDateTo.Date.Day < productModel.AvailabilityDateFrom.Date.Day)
            {
                return new BaseResponse<ProductDto>{
                    Message = "You Tender Invalid Product Availability ⚠ ",
                    IsSuccess = false
                };
            }
             if(productModel.AvailabilityDateTo.Date > productModel.AvailabilityDateFrom.Date.AddDays(7))
            {
                return new BaseResponse<ProductDto>{
                    Message = "Your product availability must not be more than 7 days ⚠ ",
                    IsSuccess = false
                };
            }
            var farmerEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            var farmer = _farmerRepository.GetByEmailAsync(farmerEmail);

            var product = new Product{
                 FarmerId = farmer.Id,
                 FirstDimentionPicture = productModel.FirstDimentionPicture,
                 SecondDimentionPicture = productModel.SecondDimentionPicture,
                 ThirdDimentionPicture = productModel.ThirdDimentionPicture,
                 ForthDimentionPicture = productModel.ForthDimentionPicture,
                 ProductName = productModel.ProductName.Trim(),
                 FarmerUserName = farmer.User.UserName,
                 FarmerEmail = farmer.User.Email,
                 Quantity = productModel.Quantity,
                 Price = productModel.Price,
                 Measurement = productModel.Measurement,
                 AvailabilityDateFrom = productModel.AvailabilityDateFrom,
                 AvailabilityDateTo = productModel.AvailabilityDateTo,
                 DateCreated = DateTime.Now,
                 ProductLocalGovernment = farmer.User.Address.LocalGovernment,
                 ProductState = farmer.User.Address.State,
                 IsAvailable = true,
                 FarmerRank = farmer.Ranking
            };
              var productt = await _productRepository.CreateAsync(product);
           var productDto = new ProductDto{
                 Id = productt.Id,
                 FarmerId = product.FarmerId,
                 FirstDimentionPicture = product.FirstDimentionPicture,
                 SecondDimentionPicture = product.SecondDimentionPicture,
                 ThirdDimentionPicture = product.ThirdDimentionPicture,
                 ForthDimentionPicture = product.ForthDimentionPicture,
                 ProductName = product.ProductName,
                 FarmerUserName = product.FarmerUserName,
                 FarmerEmail = product.FarmerEmail,
                 Quantity = product.Quantity,
                 Price = product.Price,
                 Measurement = product.Measurement,
                 AvailabilityDateFrom = product.AvailabilityDateFrom,
                 AvailabilityDateTo = product.AvailabilityDateTo,
                 DateCreated = product.DateCreated,
                 ProductLocalGovernment = product.ProductLocalGovernment,
                 ProductState = product.ProductState,
                 IsAvailable = product.IsAvailable

           };
             return new BaseResponse<ProductDto>{
                IsSuccess = true,
                Message = "Farm Product Created successfully 😎",
                Data = productDto
            };
        }

        public async Task DeleteExpiredProducts()
        {
           await _productRepository.DeleteExpiredProducts();
        }

        public async Task DeleteProduct(string productId)
        {
            var product = _productRepository.GetProductById(productId);
            await  _productRepository.DeleteProduct(product);
        }

        public async Task<BaseResponse<IEnumerable<ProductDto>>> GetAllFarmProductAsync()
        {
            var products = await _productRepository.GetAllFarmProductAsync();
            if(products == null)
            {
                  return new BaseResponse<IEnumerable<ProductDto>>{
                Message = "Farm product not found 🙄",
                IsSuccess = false
            };
            
            }
            var productDto = products.Select(product => new ProductDto{
                 Id = product.Id,
                 FarmerId = product.FarmerId,
                 FirstDimentionPicture = product.FirstDimentionPicture,
                 SecondDimentionPicture = product.SecondDimentionPicture,
                 ThirdDimentionPicture = product.ThirdDimentionPicture,
                 ForthDimentionPicture = product.ForthDimentionPicture,
                 ProductName = product.ProductName,
                 FarmerUserName = product.FarmerUserName,
                 FarmerEmail = product.FarmerEmail,
                 Quantity = product.Quantity,
                 Price = product.Price,
                 Measurement = product.Measurement,
                 AvailabilityDateFrom = product.AvailabilityDateFrom,
                 AvailabilityDateTo = product.AvailabilityDateTo,
                 DateCreated = product.DateCreated,
                 ProductLocalGovernment = product.ProductLocalGovernment,
                 ProductState = product.ProductState,
                 IsAvailable = product.IsAvailable

            }).ToList();

            return new BaseResponse<IEnumerable<ProductDto>>{
                Message = "Farm product retrived successfully 😎",
                IsSuccess = true,
                Data = productDto
            };
        }

        public async Task<BaseResponse<IEnumerable<ProductDto>>> GetAllFarmProductByLocationAsync()
        {
             var userEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
              var farmerId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userRepository.GetByEmailAsync(userEmail);
            var products = await _productRepository.GetAllFarmProductByLocationAsync(user.Address.LocalGovernment, user);
              if(products == null)
            {
                    return new BaseResponse<IEnumerable<ProductDto>>{
                    Message = "Farm product not found 🙄",
                    IsSuccess = false
                };
            }
            var productDto = products.Select(product => new ProductDto{
                 Id = product.Id,
                 FarmerId = product.FarmerId,
                 FirstDimentionPicture = product.FirstDimentionPicture,
                 SecondDimentionPicture = product.SecondDimentionPicture,
                 ThirdDimentionPicture = product.ThirdDimentionPicture,
                 ForthDimentionPicture = product.ForthDimentionPicture,
                 ProductName = product.ProductName,
                 FarmerUserName = product.FarmerUserName,
                 FarmerEmail = product.FarmerEmail,
                 Quantity = product.Quantity,
                 Price = product.Price,
                 Measurement = product.Measurement,
                 AvailabilityDateFrom = product.AvailabilityDateFrom,
                 AvailabilityDateTo = product.AvailabilityDateTo,
                 DateCreated = product.DateCreated,
                 ProductLocalGovernment = product.ProductLocalGovernment,
                 ProductState = product.ProductState,
                 IsAvailable = product.IsAvailable,
                 FarmerRank = product.FarmerRank,
                 ThumbUp = product.ThumbUp,
                 ThumbDown = product.ThumbDown,
                 

            }).ToList();

            return new BaseResponse<IEnumerable<ProductDto>>{
                Message = "Farm product retrived successfully 😎",
                IsSuccess = true,
                Data = productDto
            };
        }

        public async Task<BaseResponse<IEnumerable<ProductDto>>> GetFarmerFarmProductsByIdAsync()
        {
             var userEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            var farmer = _farmerRepository.GetByEmailAsync(userEmail);
            var products = await _productRepository.GetFarmerFarmProductsByIdAsync(farmer.Id);
                if(products == null)
            {
                    return new BaseResponse<IEnumerable<ProductDto>>{
                    Message = "Farm product not found 🙄",
                    IsSuccess = false
                };
            }
            var productDto = products.Select(product => new ProductDto{
                 Id = product.Id,
                 FarmerId = product.FarmerId,
                 FirstDimentionPicture = product.FirstDimentionPicture,
                 SecondDimentionPicture = product.SecondDimentionPicture,
                 ThirdDimentionPicture = product.ThirdDimentionPicture,
                 ForthDimentionPicture = product.ForthDimentionPicture,
                 ProductName = product.ProductName,
                 FarmerUserName = product.FarmerUserName,
                 FarmerEmail = product.FarmerEmail,
                 Quantity = product.Quantity,
                 Price = product.Price,
                 Measurement = product.Measurement,
                 AvailabilityDateFrom = product.AvailabilityDateFrom,
                 AvailabilityDateTo = product.AvailabilityDateTo,
                 DateCreated = product.DateCreated,
                 ProductLocalGovernment = product.ProductLocalGovernment,
                 ProductState = product.ProductState,
                 IsAvailable = product.IsAvailable

            }).ToList();

            return new BaseResponse<IEnumerable<ProductDto>>{
                Message = "Farm product retrived successfully 😎",
                IsSuccess = true,
                Data = productDto
            };
        }

        public async Task<BaseResponse<ProductDto>> GetProductById(string productId)
        {
            var product =_productRepository.GetProductById(productId);
            if(product == null)
            {
                    return new BaseResponse<ProductDto>{
                    Message = "Farm product not found 🙄",
                    IsSuccess = false
                };
            }
            var productDto = new ProductDto{
                 Id = product.Id,
                 FarmerId = product.FarmerId,
                 FirstDimentionPicture = product.FirstDimentionPicture,
                 SecondDimentionPicture = product.SecondDimentionPicture,
                 ThirdDimentionPicture = product.ThirdDimentionPicture,
                 ForthDimentionPicture = product.ForthDimentionPicture,
                 ProductName = product.ProductName,
                 FarmerUserName = product.FarmerUserName,
                 FarmerEmail = product.FarmerEmail,
                 Quantity = product.Quantity,
                 Price = product.Price,
                 Measurement = product.Measurement,
                 AvailabilityDateFrom = product.AvailabilityDateFrom,
                 AvailabilityDateTo = product.AvailabilityDateTo,
                 DateCreated = product.DateCreated,
                 ProductLocalGovernment = product.ProductLocalGovernment,
                 ProductState = product.ProductState,
                 IsAvailable = product.IsAvailable

            };
            return new BaseResponse<ProductDto>{
                Message = "Product retrieved successfully 😎",
                IsSuccess = true,
                Data = productDto
            };
        }

        public async Task<BaseResponse<IEnumerable<ProductDto>>> SearchProductsByProductNameOrFarmerUserNameOrFarmerEmail(string searchInput)
        {
               var userEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
               var user = _userRepository.GetByEmailAsync(userEmail);
             var products = await _productRepository.SearchProductsByProductNameOrFarmerUserNameOrFarmerEmail(searchInput, user);
             if(products == null)
            {
                    return new BaseResponse<IEnumerable<ProductDto>>{
                    Message = "Farm product not found 🙄",
                    IsSuccess = false
                };
            }
            var productDto = products.Select(product => new ProductDto{
                 Id = product.Id,
                 FarmerId = product.FarmerId,
                 FirstDimentionPicture = product.FirstDimentionPicture,
                 SecondDimentionPicture = product.SecondDimentionPicture,
                 ThirdDimentionPicture = product.ThirdDimentionPicture,
                 ForthDimentionPicture = product.ForthDimentionPicture,
                 ProductName = product.ProductName,
                 FarmerUserName = product.FarmerUserName,
                 FarmerEmail = product.FarmerEmail,
                 Quantity = product.Quantity,
                 Price = product.Price,
                 Measurement = product.Measurement,
                 AvailabilityDateFrom = product.AvailabilityDateFrom,
                 AvailabilityDateTo = product.AvailabilityDateTo,
                 DateCreated = product.DateCreated,
                 ProductLocalGovernment = product.ProductLocalGovernment,
                 ProductState = product.ProductState,
                 IsAvailable = product.IsAvailable

            }).ToList();

            return new BaseResponse<IEnumerable<ProductDto>>{
                Message = "Farm product retrived successfully 😎",
                IsSuccess = true,
                Data = productDto
            };
        }

        public async Task ThumbDown(string productId)
        {
            var product = _productRepository.GetProductById(productId);
            var email =  _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
           if(product.FarmerEmail  != email)
           {
                product.ThumbDown++;
                _productRepository.UpdateProduct(product);
           }
        }

        public async Task ThumbUp(string productId)
        {
            var product = _productRepository.GetProductById(productId);
            var email =  _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
           if(product.FarmerEmail != email)
           {
                product.ThumbUp++;
                _productRepository.UpdateProduct(product);
           }
        }

        public async Task<BaseResponse<ProductDto>> UpdateProduct(UpdateProductRequestModel productModel, string productId)
        {
            var product = _productRepository.GetProductById(productId);
            if(product == null)
            {
                 return new BaseResponse<ProductDto>{
                Message = "Farm product not updated,internal error ⚠ ",
                IsSuccess = false
            };
            }
            product.ProductName = productModel.ProductName ?? product.ProductName;
            product.Quantity = productModel.Quantity != product.Quantity? productModel.Quantity: product.Quantity;
            product.Price = productModel.Price != product.Price? productModel.Price : product.Price;
            product.Measurement = productModel.Measurement != product.Measurement ? productModel.Measurement : product.Measurement;
            product.AvailabilityDateFrom = productModel.AvailabilityDateFrom != product.AvailabilityDateFrom? productModel.AvailabilityDateFrom: product.AvailabilityDateFrom;
            product.AvailabilityDateTo = productModel.AvailabilityDateTo != product.AvailabilityDateTo ? productModel.AvailabilityDateTo : product.AvailabilityDateTo;
            product.DateModified = DateTime.Now;
            _productRepository.UpdateProduct(product);

             var email = new EmailRequestModel{
                 ReceiverEmail = product.FarmerEmail,
                 ReceiverName = product.FarmerUserName,
                 Subject = "Product Updated",
                 Message = $"Hello!,Your {product.ProductName} Have been updated successfully on your portal,check your portal for confirmation.For any complain or clearification contact 08087054632 or reply to this message"
               };
               await _emailSender.SendEmail(email);


             var productDto = new ProductDto{
                 Id = product.Id,
                 FarmerId = product.FarmerId,
                 FirstDimentionPicture = product.FirstDimentionPicture,
                 SecondDimentionPicture =product.SecondDimentionPicture,
                 ThirdDimentionPicture = product.ThirdDimentionPicture,
                 ForthDimentionPicture = product.ForthDimentionPicture,
                 ProductName = product.ProductName,
                 FarmerUserName = product.FarmerUserName,
                 FarmerEmail = product.FarmerEmail,
                 Quantity = product.Quantity,
                 Price = product.Price,
                 Measurement = product.Measurement,
                 AvailabilityDateFrom = product.AvailabilityDateFrom,
                 AvailabilityDateTo = product.AvailabilityDateTo,
                 DateCreated = product.DateCreated,
                 ProductLocalGovernment = product.ProductLocalGovernment,
                 ProductState = product.ProductState,
                 IsAvailable = product.IsAvailable

            };
            return new BaseResponse<ProductDto>{
                Message = "Farm product updated successfully 😎",
                IsSuccess = true,
                Data = productDto
            };
        }
}
