using LibrarySeatReservation.Web.Models.ViewModel;

namespace LibrarySeatReservation.Web.Services;

public interface IReservationService
{
    Task<(bool Success, string ErrorMessage)> CreateReservationAsync(int userId, int seatId, DateTime date, string timeSlot);
    Task<List<MyReservationItem>> GetMyReservationsAsync(int userId);
    Task<(bool Success, string ErrorMessage)> CancelReservationAsync(int reservationId, int userId);
}
