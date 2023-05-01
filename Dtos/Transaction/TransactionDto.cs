namespace AgroExpressAPI.Dtos.Transaction;
    public class TransactionDto
    {
          public double Amount {get; set;}
          public string? FarmerEmail{get; set;}
          public string? BuyerEmail{get; set;}
          public string DateCreated {get; set;}
          public string ReferenceNumber {get; set;}
    }
