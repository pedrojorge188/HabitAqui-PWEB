using System.ComponentModel.DataAnnotations;

namespace HabitAqui_Software.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        [Display(Name = "Nome do Estado de Inscrição")]
        public string name { get; set; }

        [Display(Name = "Descrição")]
        public string desc { get; set; }


        public ICollection<Locador> Locadores { get; set; }
    }
}