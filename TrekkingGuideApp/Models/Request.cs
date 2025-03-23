namespace TrekkingGuideApp.Models
{
    public class Request
    {
        public int Id { get; set; }
        public string UserId { get; set; } // user who is requesting
        public int ItineraryId { get; set; } // which itinerary is requested
        public string Status { get; set; } // "Pending", "Accepted", "Rejected"
        public DateTime CreatedDate { get; set; }

        // Navigation
        public Itinerary Itinerary { get; set; }
    }
}