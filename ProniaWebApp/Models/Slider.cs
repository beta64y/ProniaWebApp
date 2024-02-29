using System.ComponentModel.DataAnnotations;

namespace ProniaWebApp.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public int Offer {  get; set; }
        [MaxLength(50)]
        public string? Title { get; set; }
        [MaxLength(100)]
        public string? Description { get; set; }
        public string? Image { get; set; }
    }
}
