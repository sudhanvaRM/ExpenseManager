using Microsoft.EntityFrameworkCore;

namespace Server.Models.Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .ToTable("users")
                .Property(u => u.UserId)
                .HasColumnName("user_id");

            modelBuilder.Entity<Users>()
                .Property(u => u.Username)
                .HasColumnName("username");

            modelBuilder.Entity<Users>()
                .Property(u => u.Password)
                .HasColumnName("password");
        }
    }
}
