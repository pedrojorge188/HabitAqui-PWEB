using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HabitAqui_Software.Models
{
    public class Habitacao
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        public TypesHabitacao Type { get; set; }

        [Required]
        public decimal RentalCost { get; set; }

        [Required]
        public DateTime StartDateAvailability { get; set; }

        [Required]
        public DateTime EndDateAvailability { get; set; }

        [Required]
        public int MinimumRentalPeriod { get; set; }

        [Required]
        public int MaximumRentalPeriod { get; set; }

        [ForeignKey("Locador")]
        public int LocadorId { get; set; }

    }
}
