using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HabitAqui_Software.Models
{
    public class Habitacao
    {
        public int Id { get; set; }

        [Display(Name = "Localização", Prompt = "Insira a localização")]
        public string location { get; set; }

        [Display(Name = "Custo da Renda", Prompt = "Insira o custo da renda")]
        public float rentalCost { get; set; }

        [Display(Name = "Periodo mínimo de renda", Prompt = "Escolha o Periodo mínimo de renda em dias")]
        public int minimumRentalPeriod { get; set; }

        [Display(Name = "Periodo máximo de renda", Prompt = "Escolha o Periodo máximo de renda em dias")]
        public int maximumRentalPeriod { get; set; }

        [Display(Name = "Disponibilidade")]
        public Boolean available { get; set; }

        [Display(Name = "Avaliação")]
        public float grade { get; set; }


        public int? LocadorId { get; set; }
        public Locador? locador { get; set; }


        public ICollection<RentalContract>? rentalContracts { get; set; }


        public int? categoryId { get; set; }
        public Category? category { get; set; }


        [CustomValidation(typeof(Habitacao), nameof(ValidateStartDate))]
        [Display(Name = "Data de Início do período do aluguer", Prompt = "Escolha a data de início")]
        public DateTime startDateAvailability { get; set; }

        [Display(Name = "Data de Fim do período do aluguer", Prompt = "Escolha a data de fim")]
        public DateTime endDateAvailability { get; set; }

        public static ValidationResult ValidateStartDate(DateTime startDate, ValidationContext context)
        {
            var instance = context.ObjectInstance as Habitacao;
            if (instance != null && startDate >= instance.endDateAvailability)
            {
                return new ValidationResult("A data de início deve ser anterior à data de fim.");
            }

            return ValidationResult.Success;
        }
    }
}
