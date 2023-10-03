using System.ComponentModel.DataAnnotations;

namespace HabitAqui_Software.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        public UserProfile Profile { get; set; }

    }
}
