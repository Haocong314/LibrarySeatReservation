namespace LibrarySeatReservation.Web.Models.Entity;

public class Seat
{
    public int Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public int AreaId { get; set; }
    public string Status { get; set; } = "可用"; // 可用 / 停用
    public bool HasPower { get; set; }
    public bool HasLight { get; set; }
    public string? Description { get; set; }

    // Navigation
    public SeatArea Area { get; set; } = null!;
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
