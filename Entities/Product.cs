namespace AgroExpressAPI.Entities;
    public class Product
    {
          public string Id{get; set;} = Guid.NewGuid().ToString();
      public string FarmerId{get; set;} 
      public Farmer Farmer{get; set;}
      public byte[] FirstDimentionPicture{get; set;}
      public byte[] SecondDimentionPicture{get; set;}
      public byte[] ThirdDimentionPicture{get; set;}
      public byte[] ForthDimentionPicture{get; set;}
      public string ProductName{get; set;}
      public string FarmerUserName{get; set;}
      public string FarmerEmail{get; set;}
      public int Quantity{get; set;}
      public double Price{get; set;}
      public string Measurement{get; set;}
      public DateTime AvailabilityDateFrom{get; set;}
      public DateTime AvailabilityDateTo{get; set;}
      public DateTime DateCreated{get; set;}
      public DateTime? DateModified{get; set;}
       public string ProductLocalGovernment{get; set;}
        public string ProductState{get; set;}
      public bool IsAvailable{get;set;}
       public int FarmerRank{get;set;}
       public int ThumbUp {get; set;}
        public int ThumbDown {get; set;}
    }
