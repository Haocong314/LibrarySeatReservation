using LibrarySeatReservation.Web.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace LibrarySeatReservation.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<SeatArea> SeatAreas => Set<SeatArea>();
    public DbSet<Seat> Seats => Set<Seat>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(u => u.StudentId).IsUnique();
        });

        // Admin
        modelBuilder.Entity<Admin>(e =>
        {
            e.HasIndex(a => a.Username).IsUnique();
        });

        // SeatArea
        modelBuilder.Entity<SeatArea>(e =>
        {
            e.Property(a => a.Name).HasMaxLength(50).IsRequired();
        });

        // Seat
        modelBuilder.Entity<Seat>(e =>
        {
            e.Property(s => s.SeatNumber).HasMaxLength(20).IsRequired();
            e.Property(s => s.Status).HasMaxLength(20).IsRequired().HasDefaultValue("可用");
            e.HasOne(s => s.Area).WithMany(a => a.Seats).HasForeignKey(s => s.AreaId);
        });

        // Reservation — 预约冲突唯一约束: 同一座位 + 同一日期 + 同一时段 + 已预约 状态最多一条
        modelBuilder.Entity<Reservation>(e =>
        {
            e.Property(r => r.TimeSlot).HasMaxLength(20).IsRequired();
            e.Property(r => r.Status).HasMaxLength(20).IsRequired().HasDefaultValue("已预约");

            e.HasIndex(r => new { r.SeatId, r.ReservationDate, r.TimeSlot })
                .IsUnique()
                .HasFilter("[Status] = N'已预约'");

            e.HasOne(r => r.User).WithMany(u => u.Reservations).HasForeignKey(r => r.UserId);
            e.HasOne(r => r.Seat).WithMany(s => s.Reservations).HasForeignKey(r => r.SeatId);
        });
    }
}
