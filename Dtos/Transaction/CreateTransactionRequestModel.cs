namespace AgroExpressAPI.Dtos.Transaction;
    public class CreateTransactionRequestModel
    {
          public double Amount {get; set;}
          public string? FarmerEmail{get; set;}
          public string? BuyerEmail{get; set;}
    }
