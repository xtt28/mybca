using Microsoft.Extensions.Options;
using MyBCA.Models.Links;

namespace MyBCA.Services.Links;

public class LinkService(IOptions<LinkOptions> options) : ILinkService
{
    public IEnumerable<Link> GetLinks() => options.Value.Links;
}