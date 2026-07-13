namespace LibrarySeatReservation.Web.Models.Entity;

public class SeatArea
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Floor { get; set; }
    public int SortOrder { get; set; }

    // Navigation
    public ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
