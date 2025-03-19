namespace TrekkingGuideApp.Models
{
    public class Place
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Stores the relative path to the uploaded image.
        /// </summary>
        public string PhotoPath { get; set; } = string.Empty;

        // Navigation property to related inineraries
        public ICollection<Itinerary>? Itineraries { get; set; }
    }
}
