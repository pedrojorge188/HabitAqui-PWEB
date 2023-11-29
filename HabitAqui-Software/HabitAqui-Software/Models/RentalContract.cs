using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HabitAqui_Software.Models
{
    public class RentalContract
    {
        public int Id { get; set; }

        [CustomValidation(typeof(RentalContract), nameof(ValidateStartDate))]
        [Display(Name = "Data de Início")]
        public DateTime startDate { get; set; }

        [Display(Name = "Data de Fim")]
        public DateTime endDate { get; set; }

        [Display(Name = "Confirmação")]
        [DefaultValue(false)]
        public Boolean isConfirmed { get; set; }

        [Range(0, 5, ErrorMessage = "A avaliação deve ser entre 1 e 5.")]
        [Display(Name = "Avaliação do utilizador")]
        public int? avaliacao { get; set; }

        [Display(Name = "Habitação")]
        public int? HabitacaoId { get; set; }
        [Display(Name = "Habitação")]
        public Habitacao? habitacao { get; set; }

        [Display(Name = "Estado Entrega")]
        public int? DeliveryStatusId { get; set; }
        public DeliveryStatus? deliveryStatus { get; set; }

        [Display(Name = "Estado Recolha")]
        public int? ReceiveStatusId { get; set; }
        public ReceiveStatus? receiveStatus { get; set; }

        [Display(Name = "Utilizador")]
        public string? userId { get; set; }
        public ApplicationUser? user { get; set; }


        public static ValidationResult ValidateStartDate(DateTime startDate, ValidationContext context)
        {
            var instance = context.ObjectInstance as RentalContract;
            if (instance != null && startDate >= instance.endDate)
            {
                return new ValidationResult("A data de início deve ser anterior à data de fim.");
            }

            return ValidationResult.Success;
        }
    }
}
