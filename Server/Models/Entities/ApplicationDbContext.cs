using Microsoft.EntityFrameworkCore;

namespace Server.Models.Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for Users and Trips
        public DbSet<Users> Users { get; set; }
        public DbSet<Trip> Trips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring the Users entity
            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users"); // Maps to "users" table

                entity.Property(u => u.UserId)
                    .HasColumnName("user_id") // Maps to "user_id" column
                    .IsRequired(); // Ensures non-null values

                entity.Property(u => u.Username)
                    .HasColumnName("username") // Maps to "username" column
                    .IsRequired()
                    .HasMaxLength(255); // Optional: set a max length for the username column

                entity.Property(u => u.Password)
                    .HasColumnName("password") // Maps to "password" column
                    .IsRequired();
            });

            // Configuring the Trips entity
            modelBuilder.Entity<Trip>(entity =>
            {
                entity.ToTable("trip"); // Maps to "trip" table

                entity.Property(t => t.TripId)
                    .HasColumnName("trip_id") // Maps to "trip_id" column
                    .IsRequired()
                    .HasDefaultValueSql("gen_random_uuid()"); // Default value for UUID

                entity.Property(t => t.TripName)
                    .HasColumnName("trip_name") // Maps to "trip_name" column
                    .IsRequired()
                    .HasMaxLength(255); // Optional: set a max length for the trip name

                entity.Property(t => t.TripDate)
                    .HasColumnName("trip_date") // Maps to "trip_date" column
                    .IsRequired(); // Ensures non-null values
            });
        }
    }
}