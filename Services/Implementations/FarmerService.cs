using AgroExpressAPI.Conversion;
using AgroExpressAPI.Dtos;
using AgroExpressAPI.Dtos.AllFarmers;
using AgroExpressAPI.Dtos.Farmer;
using AgroExpressAPI.Dtos.User;
using AgroExpressAPI.Email;
using AgroExpressAPI.Entities;
using AgroExpressAPI.Repositories.Interfaces;
using AgroExpressAPI.Services.Interfaces;

namespace AgroExpressAPI.Services.Implementations;
public class FarmerService : IFarmerService
{
      private readonly IFarmerRepository _farmerRepository;
          private readonly IUserRepository _userRepository;
          private readonly IUserService _userService;
             private readonly IEmailSender _emailSender;
               private readonly IHttpContextAccessor _httpContextAccessor;
        public FarmerService(IFarmerRepository farmerRepository,IUserRepository userRepository, IUserService userService,  IEmailSender emailSender, IHttpContextAccessor httpContextAccessor)
        {
            _farmerRepository = farmerRepository;
            _userRepository = userRepository;
            _userService = userService;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
            
        }
        public async Task<BaseResponse<FarmerDto>> CreateAsync(CreateFarmerRequestModel createFarmerModel)
        {

               var response = await _emailSender.EmailValidaton(createFarmerModel.Email);
           if(response == false)
           {
              return new BaseResponse<FarmerDto>{
                IsSuccess = false,
                Message = "your email is not valid,please check.",
            };
           }
             var address = new Address{
                    FullAddress = createFarmerModel.FullAddress,
                    LocalGovernment = createFarmerModel.LocalGovernment,
                    State = createFarmerModel.State
                  };
            var user = new User{
                  UserName = createFarmerModel.UserName.Trim(),
                  ProfilePicture = createFarmerModel.ProfilePicture,
                  Name = $"{createFarmerModel.FirstName} {createFarmerModel.LastName}",
                  PhoneNumber = createFarmerModel.PhoneNumber,
                  Address = address,
                  Gender = createFarmerModel.Gender,
                  Email = createFarmerModel.Email.Trim(),
                  Password = BCrypt.Net.BCrypt.HashPassword(createFarmerModel.Password),
                  Role = "Farmer",
                  IsActive = true,
                  IsRegistered = false,
                  Haspaid = false,
                  Due = true,
                  DateCreated = DateTime.Now

            };
            var userr = await _userRepository.CreateAsync(user);

            var farmer = new Farmer{
                UserId = userr.Id,
                User =  userr
            };
            await _farmerRepository.CreateAsync(farmer);

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

             var farmerDto = new FarmerDto{
                  UserName = createFarmerModel.UserName,
                //   ProfilePicture = createFarmerModel.ProfilePicture,
                  Name = $"{createFarmerModel.FirstName} {createFarmerModel.LastName}",
                  PhoneNumber = createFarmerModel.PhoneNumber,
                  FullAddress = createFarmerModel.FullAddress,
                  LocalGovernment = createFarmerModel.LocalGovernment,
                  State = createFarmerModel.State,
                  Gender = createFarmerModel.Gender,
                  Email = createFarmerModel.Email,
                  Password = createFarmerModel.Password,
                  Role = user.Role,
                  IsActive = user.IsActive,
                  DateCreated = user.DateCreated,
            };
            return new BaseResponse<FarmerDto>{
                IsSuccess = true,
                Message = "Farmer Created successfully 😎",
                Data = farmerDto
            };
        }

        public async Task DeleteAsync(string farmerId)
        {
           var farmer = _userRepository.GetByIdAsync(farmerId);
           if(farmer.IsActive == true)
           {
             farmer.IsActive = false;
           }
           else{

           farmer.IsActive = true;
           }
            await _userRepository.Delete(farmer);
        }

        public async Task<BaseResponse<IEnumerable<FarmerDto>>> GetAllAsync()
        {
            var farmers = await _farmerRepository.GetAllAsync();

           if(farmers == null)
            {
                return new BaseResponse<IEnumerable<FarmerDto>>
                {
                    Message = "No farmer Found 🙄",
                    IsSuccess = false
                };  
            }
              var farmer = farmers.Select(a => new FarmerDto{
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
            return new BaseResponse<IEnumerable<FarmerDto>>
            {
                Message = "List of Farmers 😎",
                IsSuccess = true,
                Data = farmer
            };
        }

        public  async Task<BaseResponse<ActiveAndNonActiveFarmers>> GetAllActiveAndNonActiveAsync()
        {
             var nonActiveFarmers = await _farmerRepository.GetAllNonActiveAsync();

           if(nonActiveFarmers == null)
            {
                return new BaseResponse<ActiveAndNonActiveFarmers>
                {
                    Message = "No farmer Found 🙄",
                    IsSuccess = false
                };  
            }
              var farmer = nonActiveFarmers.Select(a => new FarmerDto{
                  Id = a.Id,
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



              var ActiveFarmers = await _farmerRepository.GetAllAsync();

           if(ActiveFarmers == null)
            {
                return new BaseResponse<ActiveAndNonActiveFarmers>
                {
                    Message = "No farmer Found 🙄",
                    IsSuccess = false
                };  
            }
              var farmerr = ActiveFarmers.Select(a => new FarmerDto{
                  Id = a.Id,
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

            var farmers = new ActiveAndNonActiveFarmers{
                ActiveFarmers = farmer,
                NonActiveFarmers = farmerr
            };




            return new BaseResponse<ActiveAndNonActiveFarmers>
            {
                Message = "List of Farmers 😎",
                IsSuccess = true,
                Data = farmers
            };
        }

        public async Task<BaseResponse<FarmerDto>> GetByEmailAsync(string farmerEmail)
        {
             var farmer = _farmerRepository.GetByEmailAsync(farmerEmail);
             if(farmer == null)
             {
                        return new BaseResponse<FarmerDto>
                    {
                        Message = "Farmer not Found 🙄",
                        IsSuccess = false
                    };
             }
            FarmerDto farmerDto = new FarmerDto();
            if(farmer is not null)
            {
                  farmerDto.Id = farmer.User.Id;
                  farmerDto.UserName = farmer.User.UserName;
                  farmerDto. ProfilePicture = farmer.User.ProfilePicture;
                  farmerDto.Name =  farmer.User.Name;
                  farmerDto.PhoneNumber =  farmer.User.PhoneNumber;
                  farmerDto.FullAddress =  farmer.User.Address.FullAddress ;
                  farmerDto.LocalGovernment =  farmer.User.Address.LocalGovernment;
                  farmerDto.State =  farmer.User.Address.State;
                  farmerDto.Gender = farmer.User.Gender;
                  farmerDto.Email = farmer.User.Email;
                //   farmerDto.Password = farmer.User.Password;
                  farmerDto.Role = farmer.User.Role;
                  farmerDto.IsActive = farmer.User.IsActive;
                  farmerDto.DateCreated = farmer.User.DateCreated;
                  farmerDto.DateModified = farmer.User.DateModified;
                  farmerDto.Ranking = farmer.Ranking;
            }
            return new BaseResponse<FarmerDto>
            {
                Message = "Farmer Found successfully 😎",
                IsSuccess = true,
                Data = farmerDto
            };
        }

        public async Task<BaseResponse<FarmerDto>> GetByIdAsync(string farmerId)
        {
             var farmer =  _farmerRepository.GetByIdAsync(farmerId);
              if(farmer == null)
             {
                        return new BaseResponse<FarmerDto>
                    {
                        Message = "Farmer not Found 🙄",
                        IsSuccess = false
                    };
             }
            var farmerDto = new FarmerDto{
                    Id = farmer.Id,
                     UserName = farmer.User.UserName,
                     ProfilePicture =  farmer.User.ProfilePicture,
                     Name =  farmer.User.Name,
                     PhoneNumber =  farmer.User.PhoneNumber,
                     FullAddress =  farmer.User.Address.FullAddress ,
                     LocalGovernment =  farmer.User.Address.LocalGovernment,
                     State =  farmer.User.Address.State,
                     Gender = farmer.User.Gender,
                     Email = farmer.User.Email,
                    //  Password = farmer.User.Password,
                     Role = farmer.User.Role,
                     IsActive = farmer.User.IsActive,
                     DateCreated = farmer.User.DateCreated,
                     DateModified = farmer.User.DateModified

            };
            return new BaseResponse<FarmerDto>
            {
                Message = "Farmer Found successfully",
                IsSuccess = true,
                Data = farmerDto
            };
        }

        public async Task<BaseResponse<FarmerDto>> UpdateAsync(UpdateFarmerRequestModel updateFarmerModel, string id)
        {

            var updateFarmer = new UpdateUserRequestModel{
                UserName = updateFarmerModel.UserName,
                Name = updateFarmerModel.Name,
                PhoneNumber  = updateFarmerModel.PhoneNumber,
                FullAddress = updateFarmerModel.FullAddress,
                LocalGovernment = updateFarmerModel.LocalGovernment,
                State  =updateFarmerModel.State,
                Gender = updateFarmerModel.Gender,
               Email = updateFarmerModel.Email,
               Password = (updateFarmerModel.Password) != null?BCrypt.Net.BCrypt.HashPassword(updateFarmerModel.Password): null,
            };
            var user = _userService.UpdateAsync(updateFarmer, id);
              if(user == null)
            {
                  return new BaseResponse<FarmerDto>{
                Message = "farmer not updated,internal error 🙄",
                IsSuccess = false
            };
            }
            var farmer = _farmerRepository.GetByEmailAsync(updateFarmerModel.Email);
            if(farmer == null)
            {
                  return new BaseResponse<FarmerDto>{
                Message = "farmer not updated,internal error 🙄",
                IsSuccess = false
            };
            }
              _farmerRepository.Update(farmer);

              var farmerDto = new FarmerDto{
                UserName = updateFarmerModel.UserName,
                Name = updateFarmerModel.Name,
                PhoneNumber  = updateFarmerModel.PhoneNumber,
                FullAddress = updateFarmerModel.FullAddress,
                LocalGovernment = updateFarmerModel.LocalGovernment,
                State  =updateFarmerModel.State,
                Gender = updateFarmerModel.Gender,
               Email = updateFarmerModel.Email,
               Password = updateFarmer.Password
            };

            return new BaseResponse<FarmerDto>{
                Message = "Farmer Updated successfully",
                IsSuccess = true,
                Data = farmerDto
            };
        }

        public async Task<BaseResponse<IEnumerable<FarmerDto>>> SearchFarmerByEmailOrUserName(string searchInput)
        {
             var farmers = await _farmerRepository.SearchFarmerByEmailOrUsername(searchInput);

           if(farmers == null)
            {
                return new BaseResponse<IEnumerable<FarmerDto>>
                {
                    Message = "No farmer Found 🙄",
                    IsSuccess = false
                };  
            }
              var farmer = farmers.Select(a => new FarmerDto{
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
            return new BaseResponse<IEnumerable<FarmerDto>>
            {
                Message = "List of Farmers 😎",
                IsSuccess = true,
                Data = farmer
            };
        }

        public async Task FarmerMonthlyDueUpdate()
        {
            if(DateTime.Now.Date.AddDays(-1).Month == DateTime.Now.Date.Month)
            {
               await _farmerRepository.FarmerMonthlyDueUpdate();
            }
        }

        public Task UpdateToHasPaidDue(string userEmail)
        {
          
          // userEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
          var user = _userRepository.GetByEmailAsync(userEmail);
          user.Due = true;
          _userRepository.Update(user);
           return null;
        
        }
}
