using AgroExpressAPI.Dtos;
using AgroExpressAPI.Dtos.Admin;
using AgroExpressAPI.Dtos.User;
using AgroExpressAPI.Repositories.Interfaces;
using AgroExpressAPI.Services.Interfaces;

namespace AgroExpressAPI.Services.Implementations;
public class AdminService : IAdminService
{
        private readonly IAdminRepository _adminRepository;
        private readonly IUserService _userService;
         private readonly IUserRepository _userRepository;
        public AdminService(IAdminRepository adminRepository, IUserService userService, IUserRepository userRepository)
        {
            _adminRepository = adminRepository;
            _userService = userService;
            _userRepository = userRepository;
        }
    public async Task DeleteAsync(string adminId)
        {
            var admin = _userRepository.GetByIdAsync(adminId);
           if(admin.IsActive == true)
           {
             admin.IsActive = false;
           }
           else{

           admin.IsActive = true;
           }
            await _userRepository.Delete(admin);
        }

        public async Task<BaseResponse<IEnumerable<AdminDto>>> GetAllAsync()
        {
             var admins = await _adminRepository.GetAllAsync();

           if(admins == null)
            {
                return new BaseResponse<IEnumerable<AdminDto>>
                {
                    Message = "No admin Found 🙄",
                    IsSuccess = false
                };  
            }
              var admin = admins.Select(a => new AdminDto{
                  UserName = a.User.UserName,
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
            return new BaseResponse<IEnumerable<AdminDto>>
            {
                Message = "List of Admins 📔",
                IsSuccess = true,
                Data = admin
            };
        }

        public async Task<BaseResponse<AdminDto>> GetByEmailAsync(string adminEmail)
        {
              var admin =  _adminRepository.GetByEmailAsync(adminEmail);
              if(admin == null)
              {
                        return new BaseResponse<AdminDto>
                    {
                        Message = "Admin not Found 🙄",
                        IsSuccess = false
                    };
              }
              var adminDto = new AdminDto();
            if(admin is not null)
            {
                  adminDto.Id = admin.User.Id;
                  adminDto.UserName = admin.User.UserName;
                  adminDto.Name =   admin.User.Name;
                  adminDto.PhoneNumber =   admin.User.PhoneNumber;
                  adminDto.FullAddress =   admin.User.Address.FullAddress ;
                  adminDto.LocalGovernment =   admin.User.Address.LocalGovernment;
                  adminDto.State =   admin.User.Address.State;
                  adminDto.Gender =  admin.User.Gender;
                  adminDto.Email =  admin.User.Email;
                //   adminDto.Password =  admin.User.Password;
                  adminDto.Role =  admin.User.Role;
                  adminDto.IsActive =  admin.User.IsActive;
                  adminDto.DateCreated =  admin.User.DateCreated;
                  adminDto.DateModified =  admin.User.DateModified;
            }
            return new BaseResponse<AdminDto>
            {
                Message = "Admin Found successfully 😎",
                IsSuccess = true,
                Data = adminDto
            };
        }

        public async Task<BaseResponse<AdminDto>> GetByIdAsync(string adminId)
        {
             var admin =  _adminRepository.GetByIdAsync(adminId);
               if(admin == null)
              {
                        return new BaseResponse<AdminDto>
                    {
                        Message = "Admin not Found 🙄",
                        IsSuccess = false
                    };
              }
            var adminDto = new AdminDto{
                     UserName = admin.User.UserName,
                     Name =  admin.User.Name,
                     PhoneNumber =  admin.User.PhoneNumber,
                     FullAddress =  admin.User.Address.FullAddress ,
                     LocalGovernment =  admin.User.Address.LocalGovernment,
                     State =  admin.User.Address.State,
                     Gender = admin.User.Gender,
                     Email = admin.User.Email,
                     Password = admin.User.Password,
                     Role = admin.User.Role,
                     IsActive = admin.User.IsActive,
                     DateCreated = admin.User.DateCreated,
                     DateModified = admin.User.DateModified

            };
            return new BaseResponse<AdminDto>
            {
                Message = "Admin Found successfully 😎",
                IsSuccess = true,
                Data = adminDto
            };
        }

        public async Task<BaseResponse<AdminDto>> UpdateAsync(UpdateAdminRequestModel updateAdminModel, string id)
        {
              var updateAdmin = new UpdateUserRequestModel{
                UserName = updateAdminModel.UserName,
                Name = updateAdminModel.Name,
                PhoneNumber  = updateAdminModel.PhoneNumber,
                FullAddress = updateAdminModel.FullAddress,
                LocalGovernment = updateAdminModel.LocalGovernment,
                State  = updateAdminModel.State,
                Gender = updateAdminModel.Gender,
               Email = updateAdminModel.Email,
               Password = (updateAdminModel.Password) != null?BCrypt.Net.BCrypt.HashPassword(updateAdminModel.Password): null,
            };
            var user =  _userService.UpdateAsync(updateAdmin, id);

            if(user.IsSuccess == false)
            {
                    return new BaseResponse<AdminDto>{
                    Message = "Admin not Updated, internal error 🙄",
                    IsSuccess = false
                };

            }
              var admin = _adminRepository.GetByEmailAsync(updateAdmin.Email);
               if(admin == null)
            {
                    return new BaseResponse<AdminDto>{
                    Message = "Admin not Updated, internal error 🙄",
                    IsSuccess = false
                };

            }
              _adminRepository.Update(admin);

              var adminDto = new AdminDto{
                UserName = updateAdminModel.UserName,
                Name = updateAdminModel.Name,
                PhoneNumber  = updateAdminModel.PhoneNumber,
                FullAddress = updateAdminModel.FullAddress,
                LocalGovernment = updateAdminModel.LocalGovernment,
                State  = updateAdminModel.State,
                Gender = updateAdminModel.Gender,
               Email = updateAdminModel.Email,
               Password = updateAdmin.Password
            };

            return new BaseResponse<AdminDto>{
                Message = "Admin Updated successfully 😎",
                IsSuccess = true,
                Data = adminDto
            };
        }
}
