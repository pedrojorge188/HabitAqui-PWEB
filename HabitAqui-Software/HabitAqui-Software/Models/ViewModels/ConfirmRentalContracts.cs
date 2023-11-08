namespace HabitAqui_Software.Models.ViewModels
{
    public class ConfirmRentalContracts
    {
        public RentalContract rentalContract { get; set; }
        public Boolean hasEquipments;
        public Boolean hasDamage;
        public string? observation;
    }
}
