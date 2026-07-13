using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace LibrarySeatReservation.Web.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;
    public UserService(AppDbContext db) => _db = db;

    public async Task<User?> GetByIdAsync(int id)
        => await _db.Users.FindAsync(id);

    public async Task<List<User>> GetAllAsync()
        => await _db.Users.OrderBy(u => u.StudentId).ToListAsync();

    public User? GetCurrentUser(HttpContext context)
    {
        var userId = context.Session.GetInt32("UserId");
        if (userId == null) return null;
        return _db.Users.Find(userId.Value);
    }

    public void SetCurrentUser(HttpContext context, int userId)
    {
        context.Session.SetInt32("UserId", userId);
    }

    public void ClearCurrentUser(HttpContext context)
    {
        context.Session.Remove("UserId");
    }
}
