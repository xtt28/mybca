using Microsoft.AspNetCore.Mvc;
using MyBCA.Models.Nutrislice;
using MyBCA.Models.Nutrislice.Responses;
using MyBCA.Services.Nutrislice;

namespace MyBCA.Controllers;

[ApiController]
[Route("api/lunch")]
public class LunchApiController(INutrisliceService menuService) : ControllerBase
{
    [EndpointSummary("Retrieves the lunch menu for the week")]
    [HttpGet("week")]
    public async Task<ActionResult<MenuWeek>> GetWeek()
    {
        var week = await menuService.GetMenuWeekAsync();
        return Ok(new NutrisliceApiResponse<MenuWeek>(week, menuService.Expiry));
    }

    [EndpointSummary("Retrieves the lunch menu for the day")]
    [HttpGet("day")]
    public async Task<ActionResult<MenuDay>> GetDay()
    {
        var day = await menuService.GetMenuDayAsync();
        if (day is null)
        {
            return NotFound();
        }

        return Ok(new NutrisliceApiResponse<MenuDay>(day, menuService.Expiry));
    }
}