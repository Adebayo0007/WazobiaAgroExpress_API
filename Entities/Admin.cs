namespace AgroExpressAPI.Entities;
public class Admin
{
     public string Id{get; set;} = Guid.NewGuid().ToString().Substring(0,36);
      public string UserId{get; set;} 
       public User User{get; set;} 
}