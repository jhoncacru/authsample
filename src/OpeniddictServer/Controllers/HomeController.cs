using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace OpeniddictServer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        //Log.Error("Startup:DefaultStringConnection : " + configuration.GetConnectionString("DefaultConnection"));
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}
