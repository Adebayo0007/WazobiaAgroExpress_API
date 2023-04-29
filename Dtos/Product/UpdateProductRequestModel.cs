namespace AgroExpressAPI.Dtos.Product;
    public class UpdateProductRequestModel
    {
      public string ProductName{get; set;}
      public int Quantity{get; set;}
      public double Price{get; set;}
      public string Measurement{get; set;}
      public DateTime AvailabilityDateFrom{get; set;}
      public DateTime AvailabilityDateTo{get; set;}
      public int ThumbUp {get; set;}
      public int ThumbDown {get; set;}
    }
