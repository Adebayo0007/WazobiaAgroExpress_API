using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AgroExpressAPI.Dtos.Product;
    public class CreateProductRequestModel
    {
      
    
      [DisplayName("Front View")]
      public string FirstDimentionPicture{get; set;}
        [DisplayName("BackView")]
      public string SecondDimentionPicture{get; set;}
        [DisplayName("Right View")]
      public string ThirdDimentionPicture{get; set;}
        [DisplayName("Left View")]
      public string ForthDimentionPicture{get; set;}
       [Required]
        [DisplayName("Product Name")]
      public string ProductName{get; set;}
       [Required]
      public int Quantity{get; set;}
       [Required]
      public double Price{get; set;}
       [Required]
      public string Measurement{get; set;}
       [Required]
        [DisplayName("Availability Date From")]
      public DateTime AvailabilityDateFrom{get; set;}
       [Required]
        [DisplayName("Availability Date To")]
      public DateTime AvailabilityDateTo{get; set;}
    }
