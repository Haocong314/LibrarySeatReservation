using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Models.Entity;
using LibrarySeatReservation.Web.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace LibrarySeatReservation.Web.Services;

public class ReservationService : IReservationService
{
    private readonly AppDbContext _db;

    public ReservationService(AppDbContext db) => _db = db;

    public async Task<(bool Success, string ErrorMessage)> CreateReservationAsync(int userId, int seatId, DateTime date, string timeSlot)
    {
        var seat = await _db.Seats.FindAsync(seatId);
        if (seat == null) return (false, "座位不存在。");
        if (seat.Status == "停用") return (false, "该座位已停用。");

        // 检查同一座位 + 同一日期 + 同一时段是否已有"已预约"状态的预约
        var conflict = await _db.Reservations.AnyAsync(r =>
            r.SeatId == seatId && r.ReservationDate == date.Date && r.TimeSlot == timeSlot && r.Status == "已预约");
        if (conflict) return (false, "该时段已被预约。");

        var reservation = new Reservation
        {
            UserId = userId,
            SeatId = seatId,
            ReservationDate = date.Date,
            TimeSlot = timeSlot,
            Status = "已预约",
            CreatedAt = DateTime.Now
        };
        _db.Reservations.Add(reservation);
        await _db.SaveChangesAsync();
        return (true, "");
    }

    public async Task<List<MyReservationItem>> GetMyReservationsAsync(int userId)
    {
        return await _db.Reservations
            .Include(r => r.Seat).ThenInclude(s => s.Area)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.ReservationDate)
            .ThenBy(r => r.TimeSlot)
            .Select(r => new MyReservationItem
            {
                Id = r.Id,
                SeatNumber = r.Seat.SeatNumber,
                AreaName = r.Seat.Area.Name,
                ReservationDate = r.ReservationDate,
                TimeSlot = r.TimeSlot,
                Status = r.Status,
                CreatedAt = r.CreatedAt
            }).ToListAsync();
    }

    public async Task<(bool Success, string ErrorMessage)> CancelReservationAsync(int reservationId, int userId)
    {
        var res = await _db.Reservations.FindAsync(reservationId);
        if (res == null) return (false, "预约记录不存在。");
        if (res.UserId != userId) return (false, "无权取消他人预约。");
        if (res.Status == "已取消") return (false, "该预约已取消。");

        res.Status = "已取消";
        res.CancelledAt = DateTime.Now;
        await _db.SaveChangesAsync();
        return (true, "");
    }
}
