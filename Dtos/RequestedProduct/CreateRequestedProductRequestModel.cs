using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AgroExpressAPI.Dtos.RequestedProduct;
    public class CreateRequestedProductRequestModel
    {
        public string ProductId {get; set;}
        [DisplayName("Buyer Email")]
        public string BuyerEmail{get; set;}
        [Required]
        [DisplayName("Product Name")]
        public string ProductName{get; set;}
         [Required]
        public double Quantity{get; set;}
         [Required]
        public double Price{get; set;}
    }
