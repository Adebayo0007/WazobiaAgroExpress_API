using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AgroExpressAPI.Dtos.Buyer;
    public class UpdateBuyerRequestModel
    {
       
        public string UserName{get; set;}
        public byte[] ProfilePicture {get; set;}
        public string Name{get; set;}  
        public string PhoneNumber{get; set;}
        public string FullAddress{get; set;}
        public string LocalGovernment {get; set;}
        public string State {get; set;}
        public string Gender{get; set;}
        public string Email{get; set;}
        public string? Password{get; set;}
    }
