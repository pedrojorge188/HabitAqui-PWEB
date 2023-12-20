using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HabitAqui_Software.Models
{
    public class ApplicationUser : IdentityUser
    {

        [Display(Name = "Primeiro Nome", Prompt = "Insira o seu primeiro nome...")]
        public string  firstName { get; set; }

        [Display(Name = "Último Nome", Prompt = "\"Insira o seu último nome...")]
        public string lastName  { get; set; }

        [Display(Name = "NIF", Prompt = "Insira o seu nif...")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "O NIF deve conter exatamente 9 dígitos numéricos.")]
        public int nif {  get; set; }

        public DateTime? registerDate { get; set; }

        [Display(Name = "Disponibilidade")]
        public Boolean available { get; set; }


    }
}
