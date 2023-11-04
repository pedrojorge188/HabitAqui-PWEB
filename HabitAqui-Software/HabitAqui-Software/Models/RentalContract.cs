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

        [DefaultValue(false)]
        public Boolean isConfirmed { get; set; }


        public int? avaliacao { get; set; }


        public int? HabitacaoId { get; set; }
        public Habitacao? habitacao { get; set; }


        public int? DeliveryStatusId { get; set; }
        public DeliveryStatus? deliveryStatus { get; set; }


        public int? ReceiveStatusId { get; set; }
        public ReceiveStatus? receiveStatus { get; set; }

        
        public int? UserId { get; set; }
        public ApplicationUser? user { get; set; }


        public int? UserTesteId { get; set; }
        public UserTeste? userTeste { get; set; }
    }
}
