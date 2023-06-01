namespace AgroExpressAPI.Dtos.Product;
    public class ProductDto
    {
      public string Id{get; set;}
      public string FarmerId{get; set;} 
      public string FirstDimentionPicture{get; set;}
      public string SecondDimentionPicture{get; set;}
      public string ThirdDimentionPicture{get; set;}
      public string ForthDimentionPicture{get; set;}
      public string ProductName{get; set;}
      public int Quantity{get; set;}
      public double Price{get; set;}
      public string FarmerUserName{get; set;}
      public string FarmerEmail{get; set;}
      public string Measurement{get; set;}
      public DateTime AvailabilityDateFrom{get; set;}
      public DateTime AvailabilityDateTo{get; set;}
      public DateTime DateCreated{get; set;}
      public bool IsAvailable{get;set;}
       public string ProductLocalGovernment{get; set;}
        public string ProductState{get; set;}
        public int FarmerRank{get; set;}
         public int ThumbUp {get; set;}
        public int ThumbDown {get; set;}

    }
