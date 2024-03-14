using ProniaWebApp.Models;

namespace ProniaWebApp.Areas.Admin.ViewModels.ProductViewModel
{
    public class ProductCreateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public IFormFile Image { get; set; }
        public int Rating { get; set; }
        public int DiscountPercent { get; set; }
        public int CategoryId { get; set; }
    }
}
