namespace TrekkingGuideApp.ViewModels
{
    public class ManageProfileViewModel
    {
        // always visible fields
        public required string FullName { get; set; }

        // additional fields for roles like admin/guide
        public string? Bio {  get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
