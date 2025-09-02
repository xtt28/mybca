using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyBCA.Models;
using MyBCA.Models.Home;
using MyBCA.Services.Bus;

namespace MyBCA.Controllers;

public class HomeController(ILogger<HomeController> logger, IBusService busService) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    private readonly IBusService _busService = busService;

    public async Task<IActionResult> Index()
    {
        var towns = await _busService.GetPositionsMapAsync();
        var keys = towns.Keys.ToList();
        keys.Sort();
        
        return View(new HomeOnboardingTemplate(keys));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
