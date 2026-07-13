using LibrarySeatReservation.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySeatReservation.Web.Controllers;

public class SeatController : Controller
{
    private readonly ISeatService _seatService;
    private readonly IUserService _userService;

    public SeatController(ISeatService seatService, IUserService userService)
    {
        _seatService = seatService;
        _userService = userService;
    }

    private IActionResult EnsureLoggedIn()
    {
        if (_userService.GetCurrentUser(HttpContext) == null)
            return RedirectToAction("Index", "Home");
        return null!;
    }

    // 座位列表页
    public async Task<IActionResult> Index(int? areaId)
    {
        var check = EnsureLoggedIn();
        if (check != null) return check;

        var seats = await _seatService.GetSeatsByAreaAsync(areaId);
        ViewBag.SelectedAreaId = areaId;
        ViewBag.Areas = await _seatService.GetAreasAsync();
        return View(seats);
    }

    // 座位详情页
    public async Task<IActionResult> Detail(int id, DateTime? date)
    {
        var check = EnsureLoggedIn();
        if (check != null) return check;

        var dateValue = date?.Date ?? DateTime.Today;
        var detail = await _seatService.GetSeatDetailAsync(id, dateValue);
        if (detail == null) return NotFound();

        return View(detail);
    }
}
