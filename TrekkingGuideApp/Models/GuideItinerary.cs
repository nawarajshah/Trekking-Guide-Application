namespace TrekkingGuideApp.Models
{
    public class GuideItinerary
    {
        public int Id { get; set; }
        public string? GuideId { get; set; } // FK to applicationUser
        public int TrekkingPlaceId { get; set; } // FK to TrekkingPlace
        public string? TrekkingDetails { get; set; }

        public Users? Guide {  get; set; }
        public Place? TrekkingPlace { get; set; }
    }
}
