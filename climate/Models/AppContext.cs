using Microsoft.EntityFrameworkCore;

namespace Climate.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
        protected AppDbContext(){}

        public DbSet<History> Historys { get; set; }
        public DbSet<Location> Locations { get; set; }
    }
}