using Microsoft.AspNetCore.Mvc;
using MyBCA.Models.Bus.Responses;
using MyBCA.Services.Bus;

namespace MyBCA.Controllers;

[ApiController]
[Route("api/bus")]
public class BusApiController(IBusService busService) : ControllerBase
{
    private readonly IBusService _busService = busService;

    [HttpGet("positions")]
    public async Task<ActionResult<BusApiResponse>> GetBusLocations()
    {
        var locations = await _busService.GetPositionsMapAsync();

        return Ok(new BusApiResponse(locations.Count, locations, _busService.Expiry));
    }
}