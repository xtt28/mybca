using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyBCA.Server.Models;
using MyBCA.Server.Models.Home;
using MyBCA.Server.Services.Bus;

namespace MyBCA.Server.Controllers;

public class HomeController(IBusService busService) : Controller
{
    public async Task<IActionResult> Index()
    {
        if (Request.Cookies.TryGetValue(NewTabController.TownCookieKey, out _))
        {
            return RedirectToAction("Index", "NewTab");
        }

        var townNames = await busService.GetBusNamesAsync();

        return View(new HomeOnboardingTemplate(townNames));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
