// ViewModels for LibrarySeatReservation

namespace LibrarySeatReservation.Web.Models.ViewModel;

public class SeatListItem
{
    public int Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string AreaName { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool HasPower { get; set; }
    public bool HasLight { get; set; }
    public int ActiveReservationCount { get; set; }
}

public class SeatDetail
{
    public int Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string AreaName { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool HasPower { get; set; }
    public bool HasLight { get; set; }
    public string? Description { get; set; }
    public List<string> BookedTimeSlots { get; set; } = new();
}

public class ReservationCreateViewModel
{
    public int SeatId { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string AreaName { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Today;
    public string SelectedTimeSlot { get; set; } = string.Empty;
    public List<string> AvailableTimeSlots { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

public class MyReservationItem
{
    public int Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string AreaName { get; set; } = string.Empty;
    public DateTime ReservationDate { get; set; }
    public string TimeSlot { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class AdminReservationItem
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public string SeatNumber { get; set; } = string.Empty;
    public string AreaName { get; set; } = string.Empty;
    public DateTime ReservationDate { get; set; }
    public string TimeSlot { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class AdminSeatItem
{
    public int Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string AreaName { get; set; } = string.Empty;
    public int Floor { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool HasPower { get; set; }
    public bool HasLight { get; set; }
}

public class AdminStatsViewModel
{
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
    public int TodayActiveReservations { get; set; }
    public int TodayCancelledReservations { get; set; }
    public List<AreaStatItem> AreaStats { get; set; } = new();
}

public class AreaStatItem
{
    public string AreaName { get; set; } = string.Empty;
    public int TotalSeats { get; set; }
    public int ActiveReservations { get; set; }
}
