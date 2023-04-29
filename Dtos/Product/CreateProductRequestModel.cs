namespace AgroExpressAPI.Dtos.Product;
    public class CreateProductRequestModel
    {
      public byte[] FirstDimentionPicture{get; set;}
      public byte[] SecondDimentionPicture{get; set;}
      public byte[] ThirdDimentionPicture{get; set;}
      public byte[] ForthDimentionPicture{get; set;}
      public string ProductName{get; set;}
      public int Quantity{get; set;}
      public double Price{get; set;}
      public string Measurement{get; set;}
      public DateTime AvailabilityDateFrom{get; set;}
      public DateTime AvailabilityDateTo{get; set;}
    }
