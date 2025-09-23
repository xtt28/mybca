using Microsoft.AspNetCore.Mvc;
using MyBCA.Shared.Models.Nutrislice;
using MyBCA.Shared.Models.Nutrislice.Responses;
using MyBCA.Server.Services.Nutrislice;
using Microsoft.AspNetCore.Cors;

namespace MyBCA.Server.Controllers;

[ApiController]
[Route("api/lunch/[action]")]
public class LunchApiController(INutrisliceService menuService) : ControllerBase
{
    [EndpointSummary("Retrieves the lunch menu for the week")]
    [EnableCors]
    [HttpGet]
    public async Task<ActionResult<MenuWeek>> Week()
    {
        var week = await menuService.GetMenuWeekAsync();
        return Ok(new NutrisliceApiResponse<MenuWeek>(week, menuService.Expiry));
    }

    [EndpointSummary("Retrieves the lunch menu for the day")]
    [EnableCors]
    [HttpGet]
    public async Task<ActionResult<MenuDay>> Day()
    {
        var day = await menuService.GetMenuDayAsync();
        if (day is null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                type: $"/errors/MenuDayNotFound",
                title: "Resource Not Found",
                detail: "Menu data for this week not found.",
                instance: HttpContext.Request.Path
            );
        }

        return Ok(new NutrisliceApiResponse<MenuDay>(day, menuService.Expiry));
    }
}