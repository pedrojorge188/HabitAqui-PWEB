namespace HabitAqui_Software.Models.ViewModels
{
    public class ConfirmRentalContracts
    {
        public RentalContract rentalContract { get; set; }
        public Boolean hasEquipments { get; set; }
        public Boolean hasDamage { get; set; }
        public string? observation { get; set; }
    }
}
