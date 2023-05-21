namespace AgroExpressAPI.Dtos.User;
    public class UserDto
    {
        
        public string Id{get; set;}
        public string UserName{get; set;}
        public string ProfilePicture {get; set;}
        public string Name{get; set;}
        public string PhoneNumber{get; set;}
        public string FullAddress{get; set;}
        public string LocalGovernment {get; set;}
        public string State {get; set;}
        public string Gender{get; set;}
        public string Email{get; set;}
        public string Password{get; set;}
        public string Role{get; set;}
         public bool Haspaid{get; set;}
        public bool Due{get; set;}
        public bool IsActive{get; set;}
        public DateTime DateCreated{get; set;}
        public DateTime? DateModified{get; set;}
    }
