using System.ComponentModel.DataAnnotations;

namespace HabitAqui_Software.Models
{
    public class Locador
    {
        public int Id { get; set; }

        [Display(Name = "Nome", Prompt = "Insira o nome do locador")]
        public string name { get; set; }

        [Display(Name = "Empresa/Companhia", Prompt = "Insira o nome da empresa que este pertence")]
        public string company { get; set; }

        [Display(Name = "Morada", Prompt = "Insira a morada do locador")]
        public string address { get; set; }

        [Display(Name = "Email", Prompt = "Insira o email do locador")]
        public string email { get; set; }

        public ICollection<Habitacao>? Habitacoes { get; set; }

        [Display(Name = "Subscrição do locador")]
        public int enrollmentId { get; set; }
        public Enrollment? enrollment { get; set; }

    }
}
