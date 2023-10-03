using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HabitAqui_Software.Models
{
    public class RentalContract
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [ForeignKey("Habitacao")]
        public int HabitacaoId { get; set; }

        [ForeignKey("User")]
        public int ClientId { get; set; }
    }
}
