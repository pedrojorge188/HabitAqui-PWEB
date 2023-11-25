using HabitAqui_Software.Models;

namespace HabitAqui_Software.Models.ViewModels
{
    public class UserWithRolesViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
        public bool IsAssociatedWithRentals { get; set; }
    }
}
