using ProniaWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ProniaWebApp.Areas.Admin.ViewModels.ProductViewModel
{
    public class ProductCreateViewModel
    {
        [Required( ErrorMessage = "Adini yaz"),MaxLength(50)]
        public string Name { get; set; }
        [Required,MaxLength(100)]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public IFormFile Image { get; set; }
        [Range(0,5)]
        public int Rating { get; set; }
        [Range(0,100)]
        public int DiscountPercent { get; set; }
        public int CategoryId { get; set; }
    }
}
