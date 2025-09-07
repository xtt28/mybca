using Microsoft.AspNetCore.Mvc;
using MyBCA.Shared.Models.Bus.Responses;
using MyBCA.Server.Services.Bus;

namespace MyBCA.Server.Controllers;

[ApiController]
[Route("api/bus")]
public class BusApiController(IBusService busService) : ControllerBase
{
    [EndpointSummary("Retrieves a map of each bus to its position")]
    [HttpGet("list")]
    public async Task<ActionResult<BusApiResponse>> GetBusLocations()
    {
        var locations = await busService.GetPositionsMapAsync();

        return Ok(new BusApiResponse(locations.Count, locations, busService.Expiry));
    }
}