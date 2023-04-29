namespace AgroExpressAPI.Entities;
    public class Transaction
    {
          public string Id{get; set;} = Guid.NewGuid().ToString();
          public double Amount {get; set;}
          public string FarmerEmail{get; set;}
          public string BuyerEmail{get; set;}
          public string DateCreated {get; set;}
          public string ReferenceNumber {get; set;}
    }
