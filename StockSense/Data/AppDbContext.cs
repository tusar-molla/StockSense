using Microsoft.EntityFrameworkCore;
using StockSense.Models;

namespace StockSense.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Models.Branch> Branches { get; set; }
        public DbSet<Models.Company> Companies { get; set; }
        public DbSet<Models.Role> Roles { get; set; }
        public DbSet<Models.User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seeding the Role data
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "SuperAdmin" },
                new Role { Id = 2, Name = "Admin" },
                new Role { Id = 3, Name = "Operator" }
            );
        }
    }
}
