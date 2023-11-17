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

        [Display(Name = "Data de nascimento", Prompt = "\"Insira a sua data de nascimento...")]
        public DateTime? bornDate{ get; set; }

        [Display(Name = "NIF", Prompt = "Insira o se nif...")]
        public int nif {  get; set; }

        public DateTime? registerDate { get; set; }

        [Display(Name = "Disponibilidade")]
        public Boolean available { get; set; }


    }
}
