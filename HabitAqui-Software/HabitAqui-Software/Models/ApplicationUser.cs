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

        [Display(Name = "NIF", Prompt = "Insira o se nif...")]
        public int nif {  get; set; }

        public DateTime? registerDate { get; set; }

        [Display(Name = "Disponibilidade")]
        public Boolean available { get; set; }

        [Display(Name = "Data de Nascimento", Prompt = "Insira a sua data de nascimento...")]
        [CustomValidation(typeof(ApplicationUser), nameof(ValidarMaioridade))]
        public DateTime? bornDate { get; set; }

        public static ValidationResult ValidarMaioridade(DateTime? bornDate, ValidationContext context)
        {
            if (!bornDate.HasValue)
            {
                return new ValidationResult("Data de nascimento é obrigatória.");
            }

            var idade = DateTime.Today.Year - bornDate.Value.Year;
            if (bornDate.Value.Date > DateTime.Today.AddYears(-idade)) idade--;

            if (idade < 18)
            {
                return new ValidationResult("Deve ter pelo menos 18 anos de idade.");
            }

            return ValidationResult.Success;
        }


    }
}
