namespace AgroExpressAPI.Entities;
    public class Farmer
    {
       public string Id{get; set;} = Guid.NewGuid().ToString();
       public string UserId{get; set;} 
       public User User{get; set;} 
       public int Ranking{get; set;} 
       public IEnumerable<Product> Products{get; set;} = new HashSet<Product>();
       public IEnumerable<RequestedProduct> RequestedProducts{get; set;} = new HashSet<RequestedProduct>();
    }
