using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HabitAqui_Software.Models
{
    public class RentalContract
    {
        public int Id { get; set; }

        [Display(Name = "Start Date")]
        public DateTime startDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime endDate { get; set; }

        [Display(Name = "Habitacao to rent")]
        public Habitacao? habitacao { get; set; }

        [Display(Name = "Delivery habitacao from a client")]
        public DeliveryStatus? receiveStatus { get; set; }

        [Display(Name = "Delivery habitacao to a client")]
        public ReceiveStatus? deliveryStatus { get; set; }

        public string userId { get; set; }

        [Display(Name = "User that did the reservation")]
        public  User? user { get; set; }

        [DefaultValue(false)]
        public Boolean isConfirmed { get; set; }
    }
}
