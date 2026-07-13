namespace LibrarySeatReservation.Web.Models.Entity;

public class User
{
    public int Id { get; set; }
    public string StudentId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;

    // Navigation
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
