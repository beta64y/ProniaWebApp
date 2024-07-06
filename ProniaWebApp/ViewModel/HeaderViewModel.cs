using ProniaWebApp.Models;

namespace ProniaWebApp.ViewModel
{
    public class HeaderViewModel
    {
        public Dictionary<string, string> Settings { get; set; }
        public List<BasketItem> BasketItems { get; set; }
        public double TotalPrice { get; set; }
    }
}
