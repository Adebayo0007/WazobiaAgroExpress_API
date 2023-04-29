

namespace AgroExpressAPI.Entities;
    public class Address
    {
         public string Id{get; set;} = Guid.NewGuid().ToString();
         public string FullAddress{get; set;}
         public string LocalGovernment {get; set;}
        public string State {get; set;}
        public User? User{get; set;}
    }
