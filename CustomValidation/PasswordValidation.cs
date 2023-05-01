using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AgroExpressAPI.CustomValidation;
    public class PasswordValidation : ValidationAttribute
    {
         public override bool IsValid(object value)
        {
            var password = value as string;
            if (string.IsNullOrWhiteSpace(password)) return false;
            if(password.Length < 6) return false;
            if(!Regex.IsMatch(password, @"(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}"))return false;
            return true;
        }
        public override string FormatErrorMessage(string name)
        {
            return $"The {name} field must be at least 6 characters long,and contains both upper and lower case letter";
        }
    }
