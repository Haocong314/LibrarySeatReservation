using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace LibrarySeatReservation.Web.Services;

public class AdminService : IAdminService
{
    private readonly AppDbContext _db;

    public AdminService(AppDbContext db) => _db = db;

    public bool ValidateLogin(string username, string password, HttpContext context)
    {
        var admin = _db.Admins.FirstOrDefault(a => a.Username == username && a.PasswordHash == password);
        if (admin == null) return false;
        context.Session.SetInt32("AdminId", admin.Id);
        context.Session.SetString("AdminUsername", admin.Username);
        return true;
    }

    public bool IsLoggedIn(HttpContext context)
        => context.Session.GetInt32("AdminId") != null;

    public void Logout(HttpContext context)
    {
        context.Session.Remove("AdminId");
        context.Session.Remove("AdminUsername");
    }

    public async Task<List<AdminReservationItem>> GetReservationsAsync(DateTime? date, string? status, int? areaId, string? keyword)
    {
        var query = _db.Reservations
            .Include(r => r.User)
            .Include(r => r.Seat).ThenInclude(s => s.Area)
            .AsQueryable();

        if (date.HasValue)
            query = query.Where(r => r.ReservationDate == date.Value.Date);
        if (!string.IsNullOrEmpty(status))
            query = query.Where(r => r.Status == status);
        if (areaId.HasValue)
            query = query.Where(r => r.Seat.AreaId == areaId.Value);
        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(r =>
                r.User.StudentId.Contains(keyword) ||
                r.User.DisplayName.Contains(keyword) ||
                r.Seat.SeatNumber.Contains(keyword));

        return await query.OrderByDescending(r => r.ReservationDate)
            .ThenBy(r => r.TimeSlot)
            .Select(r => new AdminReservationItem
            {
                Id = r.Id,
                UserName = r.User.DisplayName,
                StudentId = r.User.StudentId,
                SeatNumber = r.Seat.SeatNumber,
                AreaName = r.Seat.Area.Name,
                ReservationDate = r.ReservationDate,
                TimeSlot = r.TimeSlot,
                Status = r.Status,
                CreatedAt = r.CreatedAt
            }).ToListAsync();
    }

    public async Task<(bool Success, string ErrorMessage)> CancelReservationAsync(int reservationId)
    {
        var res = await _db.Reservations.FindAsync(reservationId);
        if (res == null) return (false, "预约记录不存在。");
        if (res.Status == "已取消") return (false, "该预约已取消。");

        res.Status = "已取消";
        res.CancelledAt = DateTime.Now;
        await _db.SaveChangesAsync();
        return (true, "");
    }

    public async Task<List<AdminSeatItem>> GetAllSeatsAsync()
    {
        return await _db.Seats.Include(s => s.Area)
            .OrderBy(s => s.Area.Name).ThenBy(s => s.SeatNumber)
            .Select(s => new AdminSeatItem
            {
                Id = s.Id,
                SeatNumber = s.SeatNumber,
                AreaName = s.Area.Name,
                Floor = s.Area.Floor,
                Status = s.Status,
                HasPower = s.HasPower,
                HasLight = s.HasLight
            }).ToListAsync();
    }

    public async Task<(bool Success, string ErrorMessage)> ToggleSeatStatusAsync(int seatId)
    {
        var seat = await _db.Seats.FindAsync(seatId);
        if (seat == null) return (false, "座位不存在。");

        seat.Status = seat.Status == "可用" ? "停用" : "可用";
        await _db.SaveChangesAsync();
        return (true, "");
    }

    public async Task<AdminStatsViewModel> GetStatsAsync()
    {
        var today = DateTime.Today;
        return new AdminStatsViewModel
        {
            TotalSeats = await _db.Seats.CountAsync(),
            AvailableSeats = await _db.Seats.CountAsync(s => s.Status == "可用"),
            TodayActiveReservations = await _db.Reservations.CountAsync(r => r.ReservationDate == today && r.Status == "已预约"),
            TodayCancelledReservations = await _db.Reservations.CountAsync(r => r.ReservationDate == today && r.Status == "已取消"),
            AreaStats = await _db.SeatAreas.Select(a => new AreaStatItem
            {
                AreaName = a.Name,
                TotalSeats = a.Seats.Count,
                ActiveReservations = a.Seats.Sum(s => s.Reservations.Count(r => r.ReservationDate == today && r.Status == "已预约"))
            }).ToListAsync()
        };
    }
}
