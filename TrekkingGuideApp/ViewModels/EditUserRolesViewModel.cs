using Microsoft.AspNetCore.Mvc.Rendering;

namespace TrekkingGuideApp.ViewModels
{
    public class EditUserRolesViewModel
    {
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? SelectedRole { get; set; }
        public List<SelectListItem>? AvailableRoles { get; set; }
    }
}