using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BC.Auth.Server.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;        
    }

    public IActionResult Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }

        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}
