namespace HabitAqui_Software.Models
{
    public class Manager
    {
        public int Id { get; set; }
        public int LocadorId { get; set; }
        public Locador locador { get; set; }
        public string userId { get; set; }
        public ApplicationUser user { get; set; }
    }
}
