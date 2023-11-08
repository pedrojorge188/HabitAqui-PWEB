using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace HabitAqui_Software.Models
{
    public class ReceiveStatus
    {
        public int Id { get; set; }

        [Display(Name = "Tem Equipamentos")]
        public Boolean hasEquipments;

        [Display(Name = "Tem Danos")]
        public Boolean hasDamage;

        [Display(Name = "Observações", Prompt = "Escreva observações sobre o estado de recolha da habitação do cliente")]
        public string? observation;

        public string? imgLink;

        [ForeignKey("rentalContract")]
        public int? rentalContractId { get; set; }
        public RentalContract? rentalContract {  get; set; }
    }
}