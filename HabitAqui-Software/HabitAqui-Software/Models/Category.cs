using System.ComponentModel.DataAnnotations;

namespace HabitAqui_Software.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Nome", Prompt = "Insira o nome da categoria...")]
        public string name { get; set; }

        [Display(Name = "Descrição", Prompt = "Insira a descrição da categoria...")]
        public string description { get; set; }


        public ICollection<Habitacao>? habitacoes { get; set; }
    }
}
