using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace LibrarySeatReservation.Web.Data;

public static class DbInitializer
{
    /// <summary>
    /// 最小演示种子数据：体验学生账号、管理员账号、区域、座位
    /// 在 Program.cs 中通过 app.Lifetime 自动调用
    /// </summary>
    public static async Task InitializeAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // 确保数据库已创建（由 Migration 保证，此处二次兜底）
        await db.Database.EnsureCreatedAsync();

        // --- 管理员（幂等：按 Username 检查） ---
        if (!await db.Admins.AnyAsync(a => a.Username == "admin"))
        {
            db.Admins.Add(new Admin
            {
                Username = "admin",
                PasswordHash = "123456", // 明文演示用，生产环境必须哈希
                CreatedAt = DateTime.Now
            });
        }

        // --- 体验学生账号（幂等） ---
        if (!await db.Users.AnyAsync())
        {
            db.Users.AddRange(
                new User { StudentId = "2024001", DisplayName = "张三" },
                new User { StudentId = "2024002", DisplayName = "李四" },
                new User { StudentId = "2024003", DisplayName = "王五" }
            );
        }

        // --- 座位区域（幂等） ---
        if (!await db.SeatAreas.AnyAsync())
        {
            db.SeatAreas.AddRange(
                new SeatArea { Name = "自习区A", Floor = "2F", SortOrder = 1 },
                new SeatArea { Name = "自习区B", Floor = "3F", SortOrder = 2 },
                new SeatArea { Name = "电子阅览区", Floor = "2F", SortOrder = 3 }
            );
            await db.SaveChangesAsync();
        }

        // --- 座位（幂等：按 SeatNumber 检查） ---
        if (!await db.Seats.AnyAsync())
        {
            var areaA = await db.SeatAreas.FirstAsync(a => a.Name == "自习区A");
            var areaB = await db.SeatAreas.FirstAsync(a => a.Name == "自习区B");
            var areaE = await db.SeatAreas.FirstAsync(a => a.Name == "电子阅览区");

            var seats = new List<Seat>();
            for (int i = 1; i <= 8; i++)
                seats.Add(new Seat { SeatNumber = $"A-{i:D2}", AreaId = areaA.Id, HasPower = true, HasLight = true, Status = "Available" });
            for (int i = 1; i <= 8; i++)
                seats.Add(new Seat { SeatNumber = $"B-{i:D2}", AreaId = areaB.Id, HasPower = true, HasLight = true, Status = "Available" });
            for (int i = 1; i <= 4; i++)
                seats.Add(new Seat { SeatNumber = $"E-{i:D2}", AreaId = areaE.Id, HasPower = true, HasLight = false, Status = "Available" });

            // 停用一个座位用于演示
            seats[0].Status = "Disabled";

            db.Seats.AddRange(seats);
        }

        await db.SaveChangesAsync();
    }
}
