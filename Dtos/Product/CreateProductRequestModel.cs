using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AgroExpressAPI.Dtos.Product;
    public class CreateProductRequestModel
    {
      
      [Required]
      [DisplayName("Front View")]
      public IFormFile FirstDimentionPicture{get; set;}
       [Required]
        [DisplayName("BackView")]
      public IFormFile SecondDimentionPicture{get; set;}
       [Required]
        [DisplayName("Right View")]
      public IFormFile ThirdDimentionPicture{get; set;}
       [Required]
        [DisplayName("Left View")]
      public IFormFile ForthDimentionPicture{get; set;}
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
