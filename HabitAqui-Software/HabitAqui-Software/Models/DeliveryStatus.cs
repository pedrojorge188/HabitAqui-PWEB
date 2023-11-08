using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitAqui_Software.Models
{
    public class DeliveryStatus
    {
        public int Id { get; set; }

        [Display(Name = "Tem Equipamentos?")]
        public Boolean hasEquipments;

        [Display(Name = "Tem Danos")]
        public Boolean hasDamage;

        [Display(Name = "Observações", Prompt = "Escreva observações sobre o estado de entrega da habitação ao cliente...")]
        public string? observation;

        [ForeignKey("rentalContract")]
        public int? RentalContractId { get; set; }
        public RentalContract? rentalContract { get; set; }

    }
}