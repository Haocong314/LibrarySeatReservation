using LibrarySeatReservation.Web.Models.Entity;

namespace LibrarySeatReservation.Web.Services;

public interface IUserService
{
    Task<User?> GetByIdAsync(int id);
    Task<List<User>> GetAllAsync();
    User? GetCurrentUser(HttpContext context);
    void SetCurrentUser(HttpContext context, int userId);
    void ClearCurrentUser(HttpContext context);
}
