using System.ComponentModel.DataAnnotations;

namespace AgroExpressAPI.Dtos.User;
    public class LogInRequestModel
    {
        [Required]
        public string Email {get; set;}
        [Required]
        public string Password {get; set;}
    }
