


using Microsoft.EntityFrameworkCore;
using LRP_Project_Vize_MedineSarımustafaoğlu.Models;

namespace LRP_Project_Vize_MedineSarımustafaoğlu.Data
{
    public class AppDbContext : DbContext
    {
       
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        
        public AppDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=lrp.db");
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Lab> Labs { get; set; }
        public DbSet<Computer> Computers { get; set; }
    }
}