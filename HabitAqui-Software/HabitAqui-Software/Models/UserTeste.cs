namespace HabitAqui_Software.Models
{
    public class UserTeste
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public ICollection<RentalContract>? rentalContracts { get; set; }


        public int? locadorId { get; set; }
        public Locador? locador { get; set; }
    }
}
