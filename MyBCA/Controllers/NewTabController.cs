using Microsoft.AspNetCore.Mvc;
using MyBCA.Models.Bus;
using MyBCA.Models.NewTab;
using MyBCA.Services.Bus;
using MyBCA.Services.Links;
using MyBCA.Services.Nutrislice;

namespace MyBCA.Controllers;

public class NewTabController(IBusService busService, ILinkService linkService, INutrisliceService nutrisliceService) : Controller
{
    public const string TownCookieKey = "mybca_newtab_town";

    public async Task<IActionResult> Index()
    {
        var town = GetTown();

        var buses = await busService.GetPositionsMapAsync();
        NewTabBusTemplate? busTemplate = null;
        if (town != null && buses.TryGetValue(town, out var location))
        {
            var busPosition = new BusPosition(town, location);
            var busExpiry = busService.Expiry;

            busTemplate = new NewTabBusTemplate(busPosition, busExpiry);
        }

        var lunchToday = await nutrisliceService.GetMenuDayAsync();
        var lunchExpiry = nutrisliceService.Expiry;
        var lunchTemplate = lunchToday is null ? null : new NewTabLunchTemplate(lunchToday.MenuItems, lunchExpiry);

        var busList = buses?.Keys.ToList();
        busList?.Sort();

        return View(new NewTabTemplate(
            busTemplate,
            new NewTabLinksTemplate(linkService.GetLinks()),
            lunchTemplate,
            busList
        ));
    }

    private string? GetTown()
    {
        return Request.Cookies[TownCookieKey];
    }

    [HttpPost]
    public IActionResult SetTown(string name)
    {
        Response.Cookies.Append(TownCookieKey, name, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddYears(20),
            Path = "/",
            MaxAge = new TimeSpan(365 * 20, 0, 0, 0),
        });

        return RedirectToAction("Index");
    }
}