using System.ComponentModel.DataAnnotations;

namespace HabitAqui_Software.Models
{
    public class Locador
    {
        public int Id { get; set; }

        [Display(Name = "Name", Prompt = "Insert your name...")]
        public string name { get; set; }

        [Display(Name = "Company", Prompt = "Insert company name...")]
        public string company { get; set; }

        [Display(Name = "Address", Prompt = "Insert Locador Adress...")]
        public string address { get; set; }

        [Display(Name = "Email", Prompt = "Insert Locador email...")]
        public string email { get; set; }

        [Display(Name = "", Prompt = "Choose Enrollment state...")]
        public Enrollment? state { get; set; }

    }
}
