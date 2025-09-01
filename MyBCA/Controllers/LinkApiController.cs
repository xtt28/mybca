using Microsoft.AspNetCore.Mvc;
using MyBCA.Models.Links;
using MyBCA.Models.Links.Responses;
using MyBCA.Services.Links;

namespace MyBCA.Controllers;

[ApiController]
[Route("api/links")]
public class LinkApiController(ILinkService linkService) : ControllerBase
{
    private readonly ILinkService _linkService = linkService;

    [HttpGet()]
    public ActionResult<IEnumerable<Link>> GetBusLocations()
    {
        var links = _linkService.GetLinks();
        return Ok(new LinkApiResponse(links.Count(), links));
    }
}