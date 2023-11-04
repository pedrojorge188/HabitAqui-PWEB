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

        [Display(Name = "Data de inicio", Prompt = "Escolha a data de ínicio")]
        public DateTime startDateAvailability { get; set; }

        [Display(Name = "Data de Fim", Prompt = "Escolha a data de fim")]
        public DateTime endDateAvailability { get; set; }

        [Display(Name = "Periodo mínimo de renda", Prompt = "Escolha o Periodo mínimo de renda em dias")]
        public int minimumRentalPeriod { get; set; }

        [Display(Name = "Periodo máximo de renda", Prompt = "Escolha o Periodo máximo de renda em dias")]
        public int maximumRentalPeriod { get; set; }

        [Display(Name = "Disponibilidade")]
        public Boolean available { get; set; }

        [Display(Name = "Avaliação")]
        public int grade { get; set; }


        public int? LocadorId { get; set; }
        public Locador? locador { get; set; }


        public ICollection<RentalContract>? rentalContracts { get; set; }


        public int? categoryId { get; set; }
        public Category? category { get; set; }
    }
}
