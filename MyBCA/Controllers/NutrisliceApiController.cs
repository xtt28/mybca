using Microsoft.AspNetCore.Mvc;
using MyBCA.Models.Nutrislice;
using MyBCA.Models.Nutrislice.Responses;
using MyBCA.Services.Nutrislice;

namespace MyBCA.Controllers;

[ApiController]
[Route("api/lunch")]
public class LunchApiController(INutrisliceService menuService) : ControllerBase
{
    private readonly INutrisliceService _menuService = menuService;

    [HttpGet("week")]
    public async Task<ActionResult<MenuWeek>> GetWeek()
    {
        var week = await _menuService.GetMenuWeekAsync();
        return Ok(new NutrisliceApiResponse<MenuWeek>(week, _menuService.Expiry));
    }

    [HttpGet("day")]
    public async Task<ActionResult<MenuDay>> GetDay()
    {
        var day = await _menuService.GetMenuDayAsync();
        if (day is null)
        {
            return NotFound();
        }

        return Ok(new NutrisliceApiResponse<MenuDay>(day, _menuService.Expiry));
    }
}