using Microsoft.EntityFrameworkCore;

namespace Server.Models.Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for Users, Trips, and related entities
        public DbSet<Users> Users { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Trip_Participants> TripParticipants { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Debt> Debts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring the Users entity
            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.Property(u => u.UserId)
                    .HasColumnName("user_id")
                    .IsRequired();

                entity.Property(u => u.Username)
                    .HasColumnName("username")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(u => u.Password)
                    .HasColumnName("password")
                    .IsRequired();
            });

            // Configuring the Trips entity
            modelBuilder.Entity<Trip>(entity =>
            {
                entity.ToTable("trip");

                entity.Property(t => t.TripId)
                    .HasColumnName("trip_id")
                    .IsRequired()
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(t => t.TripName)
                    .HasColumnName("trip_name")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(t => t.TripDate)
                    .HasColumnName("trip_date")
                    .IsRequired();
            });

            // Configuring the Trip_Participants entity
            modelBuilder.Entity<Trip_Participants>(entity =>
            {
                entity.ToTable("trip_participants");

                entity.HasKey(tp => new { tp.TripId, tp.UserId });

                entity.Property(tp => tp.TripId)
                    .HasColumnName("trip_id")
                    .IsRequired();

                entity.Property(tp => tp.UserId)
                    .HasColumnName("user_id")
                    .IsRequired();

                entity.HasOne(tp => tp.Trip)
                    .WithMany(t => t.TripParticipants)
                    .HasForeignKey(tp => tp.TripId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(tp => tp.User)
                    .WithMany(u => u.TripParticipants)
                    .HasForeignKey(tp => tp.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuring the Expense entity
            modelBuilder.Entity<Expense>(entity =>
            {
                entity.ToTable("expense");

                entity.Property(e => e.ExpenseId)
                    .HasColumnName("expense_id")
                    .IsRequired();

                entity.Property(e => e.TripId)
                    .HasColumnName("trip_id");

                entity.Property(e => e.PaidUser)
                    .HasColumnName("paid_user")
                    .IsRequired();

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .IsRequired()
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Comment)
                    .HasColumnName("comment");

                entity.Property(e => e.Category)
                    .HasColumnName("category")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(e => e.Trip)
                    .WithMany(t => t.Expenses)
                    .HasForeignKey(e => e.TripId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.PaidUserNavigation)
                    .WithMany(u => u.Expenses)
                    .HasForeignKey(e => e.PaidUser)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuring the Debt entity
            modelBuilder.Entity<Debt>(entity =>
            {
                entity.ToTable("debt");

                entity.HasKey(d => new { d.TripId, d.DebtorId, d.CreditorId });

                entity.Property(d => d.TripId)
                    .HasColumnName("trip_id")
                    .IsRequired();

                entity.Property(d => d.DebtorId)
                    .HasColumnName("debtor_id")
                    .IsRequired();

                entity.Property(d => d.CreditorId)
                    .HasColumnName("creditor_id")
                    .IsRequired();

                entity.Property(d => d.Amount)
                    .HasColumnName("amount")
                    .IsRequired()
                    .HasColumnType("decimal(10, 2)");

                entity.Property(d => d.Status)
                    .HasColumnName("status")
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.HasOne(d => d.Trip)
                    .WithMany(t => t.Debts)
                    .HasForeignKey(d => d.TripId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Debtor)
                    .WithMany(u => u.DebtsAsDebtor)
                    .HasForeignKey(d => d.DebtorId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Creditor)
                    .WithMany(u => u.DebtsAsCreditor)
                    .HasForeignKey(d => d.CreditorId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
