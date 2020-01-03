using Microsoft.EntityFrameworkCore;

namespace super_cactus.Models
{
    public class CactusContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=cactus.db");
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().ToTable("Event");
            modelBuilder.Entity<ServerData>().ToTable("ServerData");
        }
    }
}