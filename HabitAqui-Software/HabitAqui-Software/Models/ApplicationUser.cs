using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HabitAqui_Software.Models
{
    public class ApplicationUser : IdentityUser
    {

        [Display(Name = "Primeiro Nome", Prompt = "Insira o seu primeiro nome...")]
        public string  firstName { get; set; }

        [Display(Name = "último Nome", Prompt = "\"Insira o seu último nome...")]
        public string lastName  { get; set; }
        public DateTime? bornDate{ get; set; }

        [Display(Name = "Nif", Prompt = "Insira o se nif...")]
        public int nif {  get; set; }

        public DateTime? registerDate { get; set; }

        [Display(Name = "available")]
        public Boolean available { get; set; }


    }
}
