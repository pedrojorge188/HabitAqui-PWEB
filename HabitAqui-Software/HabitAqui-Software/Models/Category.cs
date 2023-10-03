using System.ComponentModel.DataAnnotations;

namespace HabitAqui_Software.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Name", Prompt = "Insert name for the category...")]
        public string name { get; set; }

        [Display(Name = "Description", Prompt = "Insert description for the category...")]
        public string description { get; set; }
    }
}
