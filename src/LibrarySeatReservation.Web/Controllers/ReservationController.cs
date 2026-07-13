using LibrarySeatReservation.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySeatReservation.Web.Controllers;

public class ReservationController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly ISeatService _seatService;
    private readonly IUserService _userService;

    public ReservationController(IReservationService reservationService, ISeatService seatService, IUserService userService)
    {
        _reservationService = reservationService;
        _seatService = seatService;
        _userService = userService;
    }

    private IActionResult EnsureLoggedIn()
    {
        if (_userService.GetCurrentUser(HttpContext) == null)
            return RedirectToAction("Index", "Home");
        return null!;
    }

    // 预约提交页
    [HttpGet]
    public async Task<IActionResult> Create(int seatId, DateTime? date)
    {
        var check = EnsureLoggedIn();
        if (check != null) return check;

        var dateValue = date?.Date ?? DateTime.Today;
        var detail = await _seatService.GetSeatDetailAsync(seatId, dateValue);
        if (detail == null) return NotFound();

        var availableSlots = await _seatService.GetAvailableTimeSlotsAsync(seatId, dateValue);

        return View(new Models.ViewModel.ReservationCreateViewModel
        {
            SeatId = detail.Id,
            SeatNumber = detail.SeatNumber,
            AreaName = detail.AreaName,
            Date = dateValue,
            AvailableTimeSlots = availableSlots
        });
    }

    // 预约提交处理
    [HttpPost]
    public async Task<IActionResult> Create(int seatId, DateTime date, string timeSlot)
    {
        var check = EnsureLoggedIn();
        if (check != null) return check;

        var user = _userService.GetCurrentUser(HttpContext)!;
        var detail = await _seatService.GetSeatDetailAsync(seatId, date);
        var (success, error) = await _reservationService.CreateReservationAsync(user.Id, seatId, date, timeSlot);

        if (!success)
        {
            var availableSlots = await _seatService.GetAvailableTimeSlotsAsync(seatId, date);
            return View(new Models.ViewModel.ReservationCreateViewModel
            {
                SeatId = seatId,
                SeatNumber = detail?.SeatNumber ?? "",
                AreaName = detail?.AreaName ?? "",
                Date = date,
                SelectedTimeSlot = timeSlot,
                AvailableTimeSlots = availableSlots,
                ErrorMessage = error
            });
        }

        TempData["SuccessMessage"] = $"已成功预约 {detail?.SeatNumber ?? ""} {date:yyyy-MM-dd} {timeSlot}";
        return RedirectToAction("MyReservations");
    }

    // 我的预约页
    public async Task<IActionResult> MyReservations()
    {
        var check = EnsureLoggedIn();
        if (check != null) return check;

        var user = _userService.GetCurrentUser(HttpContext)!;
        var reservations = await _reservationService.GetMyReservationsAsync(user.Id);
        return View(reservations);
    }

    // 取消预约
    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        var check = EnsureLoggedIn();
        if (check != null) return check;

        var user = _userService.GetCurrentUser(HttpContext)!;
        var (success, error) = await _reservationService.CancelReservationAsync(id, user.Id);
        if (!success)
            TempData["ErrorMessage"] = error;
        else
            TempData["SuccessMessage"] = "预约已取消。";

        return RedirectToAction("MyReservations");
    }
}
