using HabitAqui_Software.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui_Software.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {

        public DbSet<Category> Categories { get; set; }
        public DbSet<DeliveryStatus> deliveryStatus { get; set; }
        public DbSet<Enrollment> enrollments { get; set; }
        public DbSet<Habitacao> habitacaos { get; set; }
        public DbSet<Locador> locador { get; set; }
        public DbSet<ReceiveStatus> receiveStatus { get; set; }
        public DbSet<RentalContract> rentalContracts { get; set; }

        public DbSet<UserTeste> userTeste { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


    }
}