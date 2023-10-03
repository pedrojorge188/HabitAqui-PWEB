using System.ComponentModel.DataAnnotations.Schema;
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

        public Boolean isConfirmed { get; set; }
    }
}
