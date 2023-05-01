using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AgroExpressAPI.CustomValidation;
    public class EmailValidation : ValidationAttribute
    {
          public override bool IsValid(object value)
        {
            var email = value as string;
            if (string.IsNullOrWhiteSpace(email)) return false;
            if(!Regex.IsMatch(email, @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                    @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + 
                    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))return false;
            return true;
        }
        public override string FormatErrorMessage(string name)
        {
            return $"The {name} is not valid,please check";
        }
    }
