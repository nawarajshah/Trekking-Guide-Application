namespace TrekkingGuideApp.Models
{
    public class Itinerary
    {
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public string GuideId { get; set; } = string.Empty;
        public string ItineraryDetails { get; set; } = string.Empty ;
        public DateTime CreateDate { get; set; } = DateTime.Now;

        // Navigation property
        public Place? Place { get; set; }
    }
}
