using System.ComponentModel.DataAnnotations;

namespace TrekkingGuideApp.ViewModels
{
    public class PlaceViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;


        [Required]
        public string Description { get; set; } = string.Empty;

        // for new photo uploads
        public IFormFile? Photo { get; set; }

        // for displaying the current photo (on edit)
        public string ExistingPhotoPath { get; set; } = string.Empty;
    }
}
