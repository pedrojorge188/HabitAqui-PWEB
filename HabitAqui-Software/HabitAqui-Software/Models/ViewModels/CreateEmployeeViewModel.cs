using System.ComponentModel.DataAnnotations;

namespace HabitAqui_Software.Models.ViewModels
{
    public class CreateEmployeeViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Primeiro nome")]
        public string firstName { set; get; }

        [Required]
        [Display(Name = "Ultimo nome")]
        public string lastName { set; get; }

        [Required]
        [Display(Name = "NIF")]
        public int nif { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; } // Funcionário ou Gestor

        public int LocadorId { get; set; }
    }
}
