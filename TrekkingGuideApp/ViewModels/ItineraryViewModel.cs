using System.ComponentModel.DataAnnotations;

namespace TrekkingGuideApp.ViewModels
{
    public class ItineraryViewModel
    {
        [Required]
        public int PlaceId { get; set; }
        [Required]
        public decimal Cost { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
