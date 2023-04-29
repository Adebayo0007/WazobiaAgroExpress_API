namespace AgroExpressAPI.Dtos.RequestedProduct;
    public class UpdateRequestedProductRequestModel
    {
        public string BuyerEmail{get; set;}
        public string ProductName{get; set;}
        public double Quantity{get; set;}
        public double Price{get; set;}
    }
