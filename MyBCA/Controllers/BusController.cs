using Microsoft.AspNetCore.Mvc;
using MyBCA.Models.Bus.Responses;
using MyBCA.Services.Bus;

namespace MyBCA.Controllers;

public class BusController(IBusService busService) : Controller
{
    public async Task<IActionResult> List()
    {
        var locationsList = await busService.GetPositionsAsync();

        return View(new BusListTemplate([], locationsList, busService.Expiry));
    }
}