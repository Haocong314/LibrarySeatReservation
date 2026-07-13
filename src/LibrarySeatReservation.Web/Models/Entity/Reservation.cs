namespace LibrarySeatReservation.Web.Models.Entity;

public class Reservation
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int SeatId { get; set; }
    public DateTime Date { get; set; }
    public string TimeSlot { get; set; } = string.Empty; // e.g. "08:00-10:00"
    public string Status { get; set; } = "Active"; // Active / Cancelled
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? CancelledAt { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public Seat Seat { get; set; } = null!;
}
