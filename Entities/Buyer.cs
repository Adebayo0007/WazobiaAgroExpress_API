namespace AgroExpressAPI.Entities;
    public class Buyer
    {
        public string Id{get; set;} = Guid.NewGuid().ToString();
        public string UserId{get; set;} 
        public User User{get; set;} 
        public IEnumerable<RequestedProduct> RequestedProducts{get; set;} = new HashSet<RequestedProduct>();
    }
