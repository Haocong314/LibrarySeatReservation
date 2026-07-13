using LibrarySeatReservation.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySeatReservation.Web.Controllers;

public class AdminController : Controller
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    // 管理员登录页
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        if (_adminService.ValidateLogin(username, password, HttpContext))
            return RedirectToAction("Reservations", "AdminManage");

        ViewBag.Error = "用户名或密码错误。";
        return View();
    }

    // 管理员退出
    public IActionResult Logout()
    {
        _adminService.Logout(HttpContext);
        return RedirectToAction("Login");
    }
}
