using Microsoft.EntityFrameworkCore;
using ConcorsoCorsoGuidaEntities;
using Microsoft.Extensions.Configuration;

namespace ConcorsoCorsoGuidaRepository
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Extraction> Extractions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../ConcorsoCorsoGuida/appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseMySQL(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<Registration>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<Extraction>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}