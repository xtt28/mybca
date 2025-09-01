using Microsoft.AspNetCore.Mvc;
using MyBCA.Models.Nutrislice;
using MyBCA.Services.Nutrislice;

namespace MyBCA.Controllers;

[ApiController]
[Route("api/lunch")]
public class LunchApiController(INutrisliceService menuService) : ControllerBase
{
    private readonly INutrisliceService _menuService = menuService;

    [HttpGet("week")]
    public async Task<MenuWeek> GetWeek()
    {
        return await _menuService.GetMenuWeekAsync();
    }

    [HttpGet("day")]
    public async Task<ActionResult<MenuDay>> GetDay()
    {
        var day = await _menuService.GetMenuDayAsync();
        if (day is null)
        {
            return NotFound();
        }

        return day;
    }
}