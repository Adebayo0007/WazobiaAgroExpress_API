using AgroExpressAPI.Dtos;
using AgroExpressAPI.Dtos.AllBuyers;
using AgroExpressAPI.Dtos.Buyer;
using AgroExpressAPI.Dtos.User;
using AgroExpressAPI.Email;
using AgroExpressAPI.Entities;
using AgroExpressAPI.Repositories.Interfaces;
using AgroExpressAPI.Services.Interfaces;

namespace AgroExpressAPI.Services.Implementations;
public class BuyerService : IBuyerService
{
         private readonly IBuyerRepository _buyerRepository;
          private readonly IUserRepository _userRepository;
          private readonly IUserService _userService;
           private readonly IEmailSender _emailSender;
        public BuyerService(IBuyerRepository buyerRepository,IUserRepository userRepository, IUserService userService, IEmailSender emailSender)
        {
            _buyerRepository = buyerRepository;
            _userRepository = userRepository;
            _userService = userService;
            _emailSender = emailSender;
        }
        public async Task<BaseResponse<BuyerDto>> CreateAsync(CreateBuyerRequestModel createBuyerModel)
        {
           var response = await _emailSender.EmailValidaton(createBuyerModel.Email);
           if(response == false)
           {
              return new BaseResponse<BuyerDto>{
                IsSuccess = false,
                Message = "your email is not valid,please check.",
            };
           }

            var address = new Address{
                    FullAddress = createBuyerModel.FullAddress,
                    LocalGovernment = createBuyerModel.LocalGovernment,
                    State = createBuyerModel.State
                  };
            var user = new User{
                  UserName = createBuyerModel.UserName.Trim(),
                  ProfilePicture = createBuyerModel.ProfilePicture,
                  Name = $"{createBuyerModel.FirstName} {createBuyerModel.LastName}",
                  PhoneNumber = createBuyerModel.PhoneNumber,
                  Address = address,
                  Gender = createBuyerModel.Gender,
                  Email = createBuyerModel.Email.Trim(),
                  Password = BCrypt.Net.BCrypt.HashPassword(createBuyerModel.Password),
                  Role = "Buyer",
                  IsActive = true,
                   IsRegistered = false,
                   Haspaid = false,
                   Due = true,
                  DateCreated = DateTime.Now

            };
            var userr = await _userRepository.CreateAsync(user);

            var buyer = new Buyer{
                UserId = userr.Id,
                User =  userr
            };
            await _buyerRepository.CreateAsync(buyer);

                 string gender = null;
               if(userr.Gender ==  "Male")
               {
                 gender="Mr";
               }
               else if(userr.Gender==  "Female")
               {
                 gender="Mrs";
               }
               else
               {
                 gender = "Mr/Mrs";
               }
               var email = new EmailRequestModel{
                 ReceiverEmail = userr.Email,
                 ReceiverName = userr.Name,
                 Subject = "Registration Confirmation",
                 Message = $"Thanks for signing up with Wazobia Agro Express {gender} {userr.Name} on {DateTime.Now.Date.ToString("dd/MM/yyyy")}.Your Registration need to be verified within today {DateTime.Now.Date.ToString("dd/MM/yyyy")} and {DateTime.Now.Date.AddDays(3).ToString("dd/MM/yyyy")} by the moderator before you can be authenticated to use the application for proper documentation and also you will recieve a mail immediately after verification.THANK YOU"
               };
               
             var mail =  await _emailSender.SendEmail(email);

             var buyerDto = new BuyerDto{
                  UserName = createBuyerModel.UserName,
                  ProfilePicture = createBuyerModel.ProfilePicture,
                  Name = $"{createBuyerModel.FirstName} {createBuyerModel.LastName}",
                  PhoneNumber = createBuyerModel.PhoneNumber,
                  FullAddress = createBuyerModel.FullAddress,
                  LocalGovernment = createBuyerModel.LocalGovernment,
                  State = createBuyerModel.State,
                  Gender = createBuyerModel.Gender,
                  Email = createBuyerModel.Email,
                  Password = createBuyerModel.Password,
                  Role = user.Role,
                  IsActive = user.IsActive,
                  DateCreated = user.DateCreated,
            };
            return new BaseResponse<BuyerDto>{
                IsSuccess = true,
                Message = "Buyer Created successfully 😎",
                Data = buyerDto
            };
        }

        public async Task DeleteAsync(string buyerId)
        {
             var buyer = _userRepository.GetByIdAsync(buyerId);
           if(buyer.IsActive == true)
           {
             buyer.IsActive = false;
           }
           else{

           buyer.IsActive = true;
           }
            await _userRepository.Delete(buyer);
        }

        public async Task<BaseResponse<ActiveAndNonActiveBuyers>> GetAllActiveAndNonActiveAsync()
        {
                  var nonActiveBuyers = await _buyerRepository.GetAllNonActiveAsync();

           if(nonActiveBuyers == null)
            {
                return new BaseResponse<ActiveAndNonActiveBuyers>
                {
                    Message = "No buyer Found 🙄",
                    IsSuccess = false
                };  
            }
              var buyer = nonActiveBuyers.Select(a => new BuyerDto{
                  UserName = a.User.UserName,
                  ProfilePicture = a.User.ProfilePicture,
                  Name = a.User.Name,
                  PhoneNumber = a.User.PhoneNumber,
                  FullAddress = a.User.Address.FullAddress ,
                  LocalGovernment = a.User.Address.LocalGovernment,
                  State = a.User.Address.State,
                  Gender = a.User.Gender,
                  Email = a.User.Email,
                  Password = a.User.Password,
                  Role = a.User.Role,
                  IsActive = a.User.IsActive,
                  DateCreated = a.User.DateCreated,
                  DateModified = a.User.DateModified
            }).ToList();



              var ActiveBuyers = await _buyerRepository.GetAllAsync();

           if(ActiveBuyers == null)
            {
                return new BaseResponse<ActiveAndNonActiveBuyers>
                {
                    Message = "No buyer Found 🙄",
                    IsSuccess = false
                };  
            }
              var buyerr = ActiveBuyers.Select(a => new BuyerDto{
                  UserName = a.User.UserName,
                  ProfilePicture = a.User.ProfilePicture,
                  Name = a.User.Name,
                  PhoneNumber = a.User.PhoneNumber,
                  FullAddress = a.User.Address.FullAddress ,
                  LocalGovernment = a.User.Address.LocalGovernment,
                  State = a.User.Address.State,
                  Gender = a.User.Gender,
                  Email = a.User.Email,
                  Password = a.User.Password,
                  Role = a.User.Role,
                  IsActive = a.User.IsActive,
                  DateCreated = a.User.DateCreated,
                  DateModified = a.User.DateModified
            }).ToList();

            var buyers = new ActiveAndNonActiveBuyers{
                ActiveBuyers = buyer,
                NonActiveBuyers = buyerr
            };

            return new BaseResponse<ActiveAndNonActiveBuyers>
            {
                Message = "List of Buyers 😎",
                IsSuccess = true,
                Data = buyers
            };
        }

        public async Task<BaseResponse<IEnumerable<BuyerDto>>> GetAllAsync()
        {
             var buyers = await _buyerRepository.GetAllAsync();

           if(buyers == null)
            {
                return new BaseResponse<IEnumerable<BuyerDto>>
                {
                    Message = "No buyer Found 🙄",
                    IsSuccess = false
                };  
            }
              var buyer = buyers.Select(a => new BuyerDto{
                  UserName = a.User.UserName,
                  ProfilePicture = a.User.ProfilePicture,
                  Name = a.User.Name,
                  PhoneNumber = a.User.PhoneNumber,
                  FullAddress = a.User.Address.FullAddress ,
                  LocalGovernment = a.User.Address.LocalGovernment,
                  State = a.User.Address.State,
                  Gender = a.User.Gender,
                  Email = a.User.Email,
                  Password = a.User.Password,
                  Role = a.User.Role,
                  IsActive = a.User.IsActive,
                  DateCreated = a.User.DateCreated,
                  DateModified = a.User.DateModified
            }).ToList();
            return new BaseResponse<IEnumerable<BuyerDto>>
            {
                Message = "List of Buyers",
                IsSuccess = true,
                Data = buyer
            };
        }

        public async Task<BaseResponse<BuyerDto>> GetByEmailAsync(string buyerEmail)
        {
             var buyer =  _buyerRepository.GetByEmailAsync(buyerEmail);
             if(buyer == null)
             {
                    return new BaseResponse<BuyerDto>
                {
                    Message = "Buyer not Found 🙄",
                    IsSuccess = false
                };

             }
             var buyerDto = new BuyerDto();
            if(buyer is not null)
            {
                  buyerDto.Id = buyer.User.Id;
                  buyerDto.UserName = buyer.User.UserName;
                  buyerDto. ProfilePicture =  buyer.User.ProfilePicture;
                  buyerDto.Name =  buyer.User.Name;
                  buyerDto.PhoneNumber =  buyer.User.PhoneNumber;
                  buyerDto.FullAddress =  buyer.User.Address.FullAddress ;
                  buyerDto.LocalGovernment =  buyer.User.Address.LocalGovernment;
                  buyerDto.State =  buyer.User.Address.State;
                  buyerDto.Gender = buyer.User.Gender;
                  buyerDto.Email = buyer.User.Email;
                //   buyerDto.Password = buyer.User.Password;
                  buyerDto.Role = buyer.User.Role;
                  buyerDto.IsActive = buyer.User.IsActive;
                  buyerDto.DateCreated = buyer.User.DateCreated;
                  buyerDto.DateModified = buyer.User.DateModified;
            }
            return new BaseResponse<BuyerDto>
            {
                Message = "Buyer Found successfully",
                IsSuccess = true,
                Data = buyerDto
            };
        }

        public async Task<BaseResponse<BuyerDto>> GetByIdAsync(string buyerId)
        {
             var buyer =  _buyerRepository.GetByIdAsync(buyerId);
                if(buyer == null)
             {
                    return new BaseResponse<BuyerDto>
                {
                    Message = "Buyer not Found 🙄",
                    IsSuccess = false
                };

             }
            var buyerDto = new BuyerDto{
                     Id = buyer.User.Id,
                     UserName = buyer.User.UserName,
                     ProfilePicture =  buyer.User.ProfilePicture,
                     Name =  buyer.User.Name,
                     PhoneNumber =  buyer.User.PhoneNumber,
                     FullAddress =  buyer.User.Address.FullAddress ,
                     LocalGovernment =  buyer.User.Address.LocalGovernment,
                     State =  buyer.User.Address.State,
                     Gender = buyer.User.Gender,
                     Email = buyer.User.Email,
                    //  Password = buyer.User.Password,
                     Role = buyer.User.Role,
                     IsActive = buyer.User.IsActive,
                     DateCreated = buyer.User.DateCreated,
                     DateModified = buyer.User.DateModified

            };
            return new BaseResponse<BuyerDto>
            {
                Message = "Buyer Found successfully",
                IsSuccess = true,
                Data = buyerDto
            };
        }

        public async Task<BaseResponse<IEnumerable<BuyerDto>>> SearchBuyerByEmailOrUserName(string searchInput)
        {
            var buyers = await _buyerRepository.SearchBuyerByEmailOrUsername(searchInput);

           if(buyers == null)
            {
                return new BaseResponse<IEnumerable<BuyerDto>>
                {
                    Message = "No buyer Found 🙄",
                    IsSuccess = false
                };  
            }
              var buyer = buyers.Select(a => new BuyerDto{
                  UserName = a.User.UserName,
                  ProfilePicture = a.User.ProfilePicture,
                  Name = a.User.Name,
                  PhoneNumber = a.User.PhoneNumber,
                  FullAddress = a.User.Address.FullAddress ,
                  LocalGovernment = a.User.Address.LocalGovernment,
                  State = a.User.Address.State,
                  Gender = a.User.Gender,
                  Email = a.User.Email,
                  Password = a.User.Password,
                  Role = a.User.Role,
                  IsActive = a.User.IsActive,
                  DateCreated = a.User.DateCreated,
                  DateModified = a.User.DateModified
            }).ToList();
            return new BaseResponse<IEnumerable<BuyerDto>>
            {
                Message = "List of Buyers",
                IsSuccess = true,
                Data = buyer
            };
        }

        public async Task<BaseResponse<BuyerDto>> UpdateAsync(UpdateBuyerRequestModel updateBuyerModel, string id)
        {
             var updateBuyer = new UpdateUserRequestModel{
                UserName = updateBuyerModel.UserName,
                ProfilePicture = updateBuyerModel.ProfilePicture ,
                Name = updateBuyerModel.Name,
                PhoneNumber  = updateBuyerModel.PhoneNumber,
                FullAddress = updateBuyerModel.FullAddress,
                LocalGovernment = updateBuyerModel.LocalGovernment,
                State  =updateBuyerModel.State,
                Gender = updateBuyerModel.Gender,
               Email = updateBuyerModel.Email,
               Password = (updateBuyerModel.Password) != null?BCrypt.Net.BCrypt.HashPassword(updateBuyerModel.Password): null,
            };
            var user = _userService.UpdateAsync(updateBuyer, id);
            if(user == null)
            {
                  return new BaseResponse<BuyerDto>{
                Message = "Buyer not updated,internal error 🙄",
                IsSuccess = false
            };
            }
            var buyer = _buyerRepository.GetByEmailAsync(updateBuyerModel.Email);
            if(buyer == null)
            {
                    return new BaseResponse<BuyerDto>{
                    Message = "Buyer not updated,internal error 🙄",
                    IsSuccess = false
                };
            }
              _buyerRepository.Update(buyer);

              var buyerDto = new BuyerDto{
                UserName = updateBuyerModel.UserName,
                ProfilePicture = updateBuyerModel.ProfilePicture ,
                Name = updateBuyerModel.Name,
                PhoneNumber  = updateBuyerModel.PhoneNumber,
                FullAddress = updateBuyerModel.FullAddress,
                LocalGovernment = updateBuyerModel.LocalGovernment,
                State  =updateBuyerModel.State,
                Gender = updateBuyerModel.Gender,
               Email = updateBuyerModel.Email,
               Password = updateBuyer.Password
            };

            return new BaseResponse<BuyerDto>{
                Message = "Buyer Updated successfully",
                IsSuccess = true,
                Data = buyerDto
            };
        }
}
