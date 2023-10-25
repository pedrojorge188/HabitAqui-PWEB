using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HabitAqui_Software.Models
{
    public class User : IdentityUser
    {

        [Display(Name = "FirstName", Prompt = "Insert your FirstName...")]
        public string  firstName { get; set; }

        [Display(Name = "LastName", Prompt = "Insert your LastName...")]
        public string lastName  { get; set; }
        public DateTime? bornDate{ get; set; }

        [Display(Name = "Nif", Prompt = "Insert nif...")]
        public int nif {  get; set; }

        public DateTime? registerDate { get; set; }

        [Display(Name = "available")]
        public Boolean available { get; set; }


        public ICollection<RentalContract>? rentalContracts { get; set; }  


        public int? locadorId { get; set; }
        public Locador? locador { get; set; }

    }
}
