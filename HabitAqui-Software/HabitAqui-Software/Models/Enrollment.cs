using System.ComponentModel.DataAnnotations;

namespace HabitAqui_Software.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        [Display(Name = "Name of Enrollment")]
        public string Name { get; set; }

        [Display(Name = "desc of Enrollment")]
        public string desc { get; set; }
    }
}