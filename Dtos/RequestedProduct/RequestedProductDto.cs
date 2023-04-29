namespace AgroExpressAPI.Dtos.RequestedProduct;
    public class RequestedProductDto
    {
         public string Id{get; set;}
        public string FarmerId{get; set;}
        public string FarmerName{get; set;}
       public string FarmerNumber{get; set;}
        public string BuyerId{get; set;}
        public string BuyerEmail{get; set;}
        public string BuyerPhoneNumber{get; set;}
        public string BuyerLocalGovernment{get; set;}
        public string ProductName{get; set;}
        public bool OrderStatus{get; set;}
         public bool IsAccepted{get; set;}
        public bool IsDelivered{get; set;}
         public bool NotDelivered{get; set;}
        public bool Haspaid{get; set;}
        public double Quantity{get; set;}
       public double Price{get; set;}
    }
