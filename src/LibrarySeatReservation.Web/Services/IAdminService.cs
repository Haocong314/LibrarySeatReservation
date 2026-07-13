using LibrarySeatReservation.Web.Models.ViewModel;

namespace LibrarySeatReservation.Web.Services;

public interface IAdminService
{
    bool ValidateLogin(string username, string password, HttpContext context);
    bool IsLoggedIn(HttpContext context);
    void Logout(HttpContext context);
    Task<List<AdminReservationItem>> GetReservationsAsync(DateTime? date, string? status, int? areaId, string? keyword);
    Task<(bool Success, string ErrorMessage)> CancelReservationAsync(int reservationId);
    Task<List<AdminSeatItem>> GetAllSeatsAsync();
    Task<(bool Success, string ErrorMessage)> ToggleSeatStatusAsync(int seatId);
    Task<AdminStatsViewModel> GetStatsAsync();
}
