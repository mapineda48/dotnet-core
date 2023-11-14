using Microsoft.EntityFrameworkCore;

namespace Agape.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        // Agrega aquí DbSet para otros modelos

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuraciones de modelo adicionales
        }
    }
}
