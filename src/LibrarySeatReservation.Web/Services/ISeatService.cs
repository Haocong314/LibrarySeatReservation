using LibrarySeatReservation.Web.Models.ViewModel;

namespace LibrarySeatReservation.Web.Services;

public interface ISeatService
{
    Task<List<SeatListItem>> GetSeatsByAreaAsync(int? areaId);
    Task<SeatDetail?> GetSeatDetailAsync(int seatId, DateTime date);
    Task<List<string>> GetAvailableTimeSlotsAsync(int seatId, DateTime date);
    Task<List<SeatAreaInfo>> GetAreasAsync();
}

public class SeatAreaInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Floor { get; set; }
}
