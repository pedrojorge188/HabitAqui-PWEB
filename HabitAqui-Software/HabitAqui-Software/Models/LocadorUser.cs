using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HabitAqui_Software.Models
{
    public class LocadorUser
    {

        [Key]
        public int LocadorUserId { get; set; }

        [ForeignKey("Locador")]
        public int LocadorId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

    }
}
