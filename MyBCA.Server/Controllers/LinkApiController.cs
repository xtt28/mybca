using Microsoft.AspNetCore.Mvc;
using MyBCA.Server.Models.Links;
using MyBCA.Server.Models.Links.Responses;
using MyBCA.Server.Services.Links;

namespace MyBCA.Server.Controllers;

[ApiController]
[Route("api/links")]
public class LinkApiController(ILinkService linkService) : ControllerBase
{
    [EndpointSummary("Retrieves a list of quick links to key BCA services")]
    [HttpGet]
    public ActionResult<IEnumerable<Link>> GetLinks()
    {
        var links = linkService.GetLinks();
        return Ok(new LinkApiResponse(links.Count(), links));
    }
}