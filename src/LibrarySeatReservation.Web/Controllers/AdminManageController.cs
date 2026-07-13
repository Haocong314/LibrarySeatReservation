using LibrarySeatReservation.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySeatReservation.Web.Controllers;

public class AdminManageController : Controller
{
    private readonly IAdminService _adminService;

    public AdminManageController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    private IActionResult EnsureAdmin()
    {
        if (!_adminService.IsLoggedIn(HttpContext))
            return RedirectToAction("Login", "Admin");
        return null!;
    }

    // 预约管理页
    public async Task<IActionResult> Reservations(DateTime? date, string? status, int? areaId, string? keyword)
    {
        var check = EnsureAdmin();
        if (check != null) return check;

        var reservations = await _adminService.GetReservationsAsync(date, status, areaId, keyword);
        ViewBag.CurrentDate = date?.ToString("yyyy-MM-dd");
        ViewBag.CurrentStatus = status;
        return View(reservations);
    }

    // 管理员取消预约
    [HttpPost]
    public async Task<IActionResult> CancelReservation(int id)
    {
        var check = EnsureAdmin();
        if (check != null) return check;

        var (success, error) = await _adminService.CancelReservationAsync(id);
        if (!success) TempData["ErrorMessage"] = error;
        else TempData["SuccessMessage"] = "预约已取消。";

        return RedirectToAction("Reservations");
    }

    // 座位管理页
    public async Task<IActionResult> Seats()
    {
        var check = EnsureAdmin();
        if (check != null) return check;

        var seats = await _adminService.GetAllSeatsAsync();
        return View(seats);
    }

    // 启用/停用座位
    [HttpPost]
    public async Task<IActionResult> ToggleSeat(int id)
    {
        var check = EnsureAdmin();
        if (check != null) return check;

        var (success, error) = await _adminService.ToggleSeatStatusAsync(id);
        if (!success) TempData["ErrorMessage"] = error;
        return RedirectToAction("Seats");
    }

    // 统计页
    public async Task<IActionResult> Stats()
    {
        var check = EnsureAdmin();
        if (check != null) return check;

        var stats = await _adminService.GetStatsAsync();
        return View(stats);
    }
}
