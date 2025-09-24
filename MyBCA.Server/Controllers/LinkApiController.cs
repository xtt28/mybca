using Microsoft.AspNetCore.Mvc;
using MyBCA.Shared.Models.Links;
using MyBCA.Shared.Models.Links.Responses;
using MyBCA.Server.Services.Links;
using Microsoft.AspNetCore.Cors;

namespace MyBCA.Server.Controllers;

[ApiController]
[Route("api/links")]
[EnableCors("AllowAll")]
public class LinkApiController(ILinkService linkService) : ControllerBase
{
    [EndpointSummary("Retrieves a list of quick links to key BCA services")]
    [HttpGet]
    public ActionResult<LinkApiResponse> GetLinks()
    {
        var links = linkService.GetLinks();
        return Ok(new LinkApiResponse(links.Count(), links));
    }
}