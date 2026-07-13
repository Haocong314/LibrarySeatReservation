using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace LibrarySeatReservation.Web.Services;

public class SeatService : ISeatService
{
    private readonly AppDbContext _db;
    private static readonly string[] AllTimeSlots = new[]
    {
        "08:00-10:00", "10:00-12:00", "12:00-14:00",
        "14:00-16:00", "16:00-18:00", "18:00-20:00", "20:00-22:00"
    };

    public SeatService(AppDbContext db) => _db = db;

    public async Task<List<SeatListItem>> GetSeatsByAreaAsync(int? areaId)
    {
        var query = _db.Seats.Include(s => s.Area).AsQueryable();
        if (areaId.HasValue)
            query = query.Where(s => s.AreaId == areaId.Value);

        return await query.Select(s => new SeatListItem
        {
            Id = s.Id,
            SeatNumber = s.SeatNumber,
            AreaName = s.Area.Name,
            Floor = s.Area.Floor,
            Status = s.Status,
            HasPower = s.HasPower,
            HasLight = s.HasLight,
            ActiveReservationCount = s.Reservations.Count(r => r.Status == "Active" && r.Date == DateTime.Today)
        }).OrderBy(s => s.AreaName).ThenBy(s => s.SeatNumber).ToListAsync();
    }

    public async Task<SeatDetail?> GetSeatDetailAsync(int seatId, DateTime date)
    {
        var seat = await _db.Seats.Include(s => s.Area).FirstOrDefaultAsync(s => s.Id == seatId);
        if (seat == null) return null;

        var bookedSlots = await _db.Reservations
            .Where(r => r.SeatId == seatId && r.Date == date.Date && r.Status == "Active")
            .Select(r => r.TimeSlot)
            .ToListAsync();

        return new SeatDetail
        {
            Id = seat.Id,
            SeatNumber = seat.SeatNumber,
            AreaName = seat.Area.Name,
            Floor = seat.Area.Floor,
            Status = seat.Status,
            HasPower = seat.HasPower,
            HasLight = seat.HasLight,
            Description = seat.Description,
            BookedTimeSlots = bookedSlots
        };
    }

    public async Task<List<string>> GetAvailableTimeSlotsAsync(int seatId, DateTime date)
    {
        var booked = await _db.Reservations
            .Where(r => r.SeatId == seatId && r.Date == date.Date && r.Status == "Active")
            .Select(r => r.TimeSlot)
            .ToListAsync();

        return AllTimeSlots.Where(t => !booked.Contains(t)).ToList();
    }

    public async Task<List<SeatAreaInfo>> GetAreasAsync()
    {
        return await _db.SeatAreas.Select(a => new SeatAreaInfo
        {
            Id = a.Id,
            Name = a.Name,
            Floor = a.Floor
        }).OrderBy(a => a.Name).ToListAsync();
    }
}
