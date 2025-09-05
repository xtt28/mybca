using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MyBCA.Models.Bus;
using MyBCA.Models.Bus.Responses;
using MyBCA.Services.Bus;

namespace MyBCA.Controllers;

public class BusController(IBusService busService) : Controller
{
    public const string FavoriteTownCookieKey = "mybca_buslist_favorites_json";

    public async Task<IActionResult> List()
    {
        var favoriteTowns = GetFavoriteTownsFromCookie();
        var favoriteLocs = new List<BusPosition>(favoriteTowns.Count);

        var locationsList = (
            await busService.GetPositionsAsync()
        )
        .ToList();
        locationsList.ForEach(loc =>
        {
            if (favoriteTowns.Contains(loc.Town))
            {
                favoriteLocs.Add(loc);
            }
        });
        locationsList.RemoveAll(favoriteLocs.Contains);
        locationsList.Sort((a, b) => string.Compare(a.Town, b.Town, StringComparison.OrdinalIgnoreCase));

        return View(new BusListTemplate(favoriteLocs, locationsList, busService.Expiry));
    }

    [HttpPost]
    public IActionResult AddFavorite(string name)
    {
        var favorites = GetFavoriteTownsFromCookie();
        favorites.Add(name);

        SaveFavoriteTownsToCookie(favorites);
        return RedirectToAction("List");
    }

    [HttpPost]
    public IActionResult RemoveFavorite(string name)
    {
        var favorites = GetFavoriteTownsFromCookie();
        favorites.Remove(name);

        SaveFavoriteTownsToCookie(favorites);
        return RedirectToAction("List");
    }

    private List<string> GetFavoriteTownsFromCookie()
    {
        if (Request.Cookies.TryGetValue(FavoriteTownCookieKey, out var cookieVal))
        {
            try
            {
                return JsonSerializer.Deserialize<List<string>>(cookieVal) ?? [];
            }
            catch
            {
                return [];
            }
        }

        return [];
    }

    private void SaveFavoriteTownsToCookie(List<string> towns)
    {
        var serialized = JsonSerializer.Serialize(towns);
        Response.Cookies.Append(FavoriteTownCookieKey, serialized, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddYears(20)
        });
    }
}