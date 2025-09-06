using Microsoft.Extensions.Options;
using MyBCA.Server.Models.Links;

namespace MyBCA.Server.Services.Links;

public class LinkService(IOptions<LinkOptions> options) : ILinkService
{
    public IEnumerable<Link> GetLinks() => options.Value.Links;
}