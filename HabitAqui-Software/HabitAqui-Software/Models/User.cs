using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HabitAqui_Software.Models
{
    public class User : IdentityUser
    {

        [Display(Name = "Username", Prompt = "Insert your username...")]
        public string username { get; set; }

        [Display(Name = "Password", Prompt = "Insert your password...")]
        public string password { get; set; }

        [Display(Name = "Email", Prompt = "Insert your email...")]
        public string email { get; set; }

        public Locador? locador { get; set; }

        public ICollection<RentalContract>? contracts { get; set; }  

    }
}
