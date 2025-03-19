using System.ComponentModel.DataAnnotations;

namespace TrekkingGuideApp.ViewModels
{
    public class ItineraryViewModel
    {
        public int PlaceId { get; set; }
        public string PlaceTitle { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter the itinerary details.")]
        public string ItineraryDetails { get; set; } = string.Empty;
    }
}
