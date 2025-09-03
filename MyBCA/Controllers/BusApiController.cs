using Microsoft.AspNetCore.Mvc;
using MyBCA.Models.Bus.Responses;
using MyBCA.Services.Bus;

namespace MyBCA.Controllers;

[ApiController]
[Route("api/bus")]
public class BusApiController(IBusService busService) : ControllerBase
{
    [HttpGet("positions")]
    public async Task<ActionResult<BusApiResponse>> GetBusLocations()
    {
        var locations = await busService.GetPositionsMapAsync();

        return Ok(new BusApiResponse(locations.Count, locations, busService.Expiry));
    }
}