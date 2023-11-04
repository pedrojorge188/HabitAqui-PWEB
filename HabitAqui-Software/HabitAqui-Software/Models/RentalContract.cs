using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HabitAqui_Software.Models
{
    public class RentalContract
    {
        public int Id { get; set; }

        [Display(Name = "Data de Início")]
        public DateTime startDate { get; set; }

        [Display(Name = "Data de Fim")]
        public DateTime endDate { get; set; }

        [Display(Name = "Confirmação")]
        [DefaultValue(false)]
        public Boolean isConfirmed { get; set; }

        [Display(Name = "Avaliação do utilizador")]
        public int? avaliacao { get; set; }

        [Display(Name = "Habitação")]
        public int? HabitacaoId { get; set; }
        [Display(Name = "Habitação")]
        public Habitacao? habitacao { get; set; }

        [Display(Name = "Estado Entrega")]
        public int? DeliveryStatusId { get; set; }
        public DeliveryStatus? deliveryStatus { get; set; }

        [Display(Name = "Estado Recolha")]
        public int? ReceiveStatusId { get; set; }
        public ReceiveStatus? receiveStatus { get; set; }

        [Display(Name = "Utilizador")]
        public int? UserId { get; set; }
        public ApplicationUser? user { get; set; }


        public int? UserTesteId { get; set; }
        public UserTeste? userTeste { get; set; }
    }
}
