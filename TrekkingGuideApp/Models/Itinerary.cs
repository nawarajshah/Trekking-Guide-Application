using System.ComponentModel.DataAnnotations.Schema;

namespace TrekkingGuideApp.Models
{
    public class Itinerary
    {
        public int Id { get; set; }
        public int? PlaceId { get; set; }
        public required string GuideId { get; set; }
        [Column(TypeName = "NUMERIC")]
        public required decimal Cost { get; set; }
        [Column("TripDuration")]
        public required string Duration { get; set; }
        public required string Description { get; set; } // rich-text initerary description
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property
        public Place? Place { get; set; }
    }
}
