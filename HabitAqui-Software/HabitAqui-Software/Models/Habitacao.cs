using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HabitAqui_Software.Models
{
    public class Habitacao
    {
        public int Id { get; set; }

        [Display(Name = "Location", Prompt = "Insert location...")]
        public string location { get; set; }

        public int? categoryId { get; set; }

        [Display(Name = "Model", Prompt = "Insert category...")]
        public Category? category { get; set; }

        [Display(Name = "RentalCost", Prompt = "Insert rental cost...")]
        public float rentalCost { get; set; }

        [Display(Name = "Start Date", Prompt = "Choose startDateAvailability ...")]
        public DateTime startDateAvailability { get; set; }

        [Display(Name = "End Date", Prompt = "Choose startDateAvailability ...")]
        public DateTime endDateAvailability { get; set; }

        [Display(Name = "Minium Rental Period", Prompt = "Choose Minium Rental Period ...")]
        public int minimumRentalPeriod { get; set; }

        [Display(Name = "Maximum Rental Period", Prompt = "Choose Maximum Rental Period ...")]
        public int maximumRentalPeriod { get; set; }

        [Display(Name = "avaliable")]
        public Boolean available { get; set; }

        [Display(Name = "grade")]
        public int grade { get; set; }

        public Locador locador { get; set; }

    }
}
