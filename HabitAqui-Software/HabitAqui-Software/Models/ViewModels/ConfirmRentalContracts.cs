namespace HabitAqui_Software.Models.ViewModels
{
    public class ConfirmRentalContracts
    {
        public RentalContract rentalContract { get; set; }
        public Boolean hasEquipments { get; set; }
        public List<String> equipments { get; set; }
        public Boolean hasDamage { get; set; }
        public string? observation { get; set; }

        public string? damageDescription { get; set; }
        public List<IFormFile> DamageImages { get; set; } = new List<IFormFile>();
    }
}
