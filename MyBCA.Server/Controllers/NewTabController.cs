using Microsoft.AspNetCore.Mvc;
using MyBCA.Shared.Models.Bus;
using MyBCA.Server.Models.NewTab;
using MyBCA.Server.Services.Bus;
using MyBCA.Server.Services.Links;
using MyBCA.Server.Services.Nutrislice;

namespace MyBCA.Server.Controllers;

public class NewTabController(ILogger<NewTabController> logger, IBusService busService, ILinkService linkService, INutrisliceService nutrisliceService) : Controller
{
    public const string TownCookieKey = "mybca_newtab_town";

    public IActionResult AddToBrowser()
    {
        return View();
    }

    public async Task<IActionResult> Index()
    {
        var town = GetTown();

        NewTabBusTemplate? busTemplate = null;
        Dictionary<string, string>? buses = null;
        try
        {
            buses = await busService.GetPositionsMapAsync();
            if (town != null && buses.TryGetValue(town, out var location))
            {
                var busPosition = new BusPosition(town, location);
                var busExpiry = busService.Expiry;

                busTemplate = new NewTabBusTemplate(busPosition, busExpiry);
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Could not get bus data: {Error}", ex);
        }

        NewTabLunchTemplate? lunchTemplate = null;
        try
        {
            var lunchToday = await nutrisliceService.GetMenuDayAsync();
            var lunchExpiry = nutrisliceService.Expiry;
            if (lunchToday is not null)
            {
                lunchTemplate = new NewTabLunchTemplate(lunchToday.MenuItems, lunchExpiry);
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Could not get Nutrislice data: {Error}", ex);
        }

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