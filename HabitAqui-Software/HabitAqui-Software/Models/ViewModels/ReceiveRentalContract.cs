using Microsoft.AspNetCore.Mvc;

namespace HabitAqui_Software.Models.ViewModels
{
    public class ReceiveRentalContract
    {
        public RentalContract rentalContract { get; set; }
        public bool hasEquipments { get; set; }
        public bool hasDamage { get; set; }
        public string? damageDescription { get; set; }
        public List<IFormFile> DamageImages { get; set; } = new List<IFormFile>();
        public string? observation { get; set; }
    }
}
