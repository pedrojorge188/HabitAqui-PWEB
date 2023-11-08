namespace HabitAqui_Software.Models
{
    public class Employer
    {
        public int Id { get; set; }
        public int LocadorId { get; set; }
        public Locador locador { get; set; }
        public ApplicationUser user { get; set; }
    }
}
