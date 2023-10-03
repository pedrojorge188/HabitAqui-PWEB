using System.ComponentModel.DataAnnotations;

namespace HabitAqui_Software.Models
{
    public class Locador
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Company { get; set; }

        public string Address { get; set; }

        [Required]
        public string Email { get; set; }

    }
}
