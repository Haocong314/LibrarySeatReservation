using LibrarySeatReservation.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySeatReservation.Web.Controllers;

public class HomeController : Controller
{
    private readonly IUserService _userService;

    public HomeController(IUserService userService)
    {
        _userService = userService;
    }

    // 用户首页
    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetAllAsync();
        return View(users);
    }

    // 体验账号切换（轻量模拟登录）
    [HttpPost]
    public IActionResult SwitchUser(int userId)
    {
        _userService.SetCurrentUser(HttpContext, userId);
        return RedirectToAction("Index", "Seat");
    }

    // 退出体验账号
    public IActionResult Logout()
    {
        _userService.ClearCurrentUser(HttpContext);
        return RedirectToAction("Index");
    }
}
