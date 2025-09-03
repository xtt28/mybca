using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyBCA.Models;
using MyBCA.Models.Home;
using MyBCA.Services.Bus;

namespace MyBCA.Controllers;

public class HomeController(IBusService busService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var towns = await busService.GetPositionsMapAsync();
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
