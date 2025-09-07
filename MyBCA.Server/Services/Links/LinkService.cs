using Microsoft.Extensions.Options;
using MyBCA.Shared.Models.Links;

namespace MyBCA.Server.Services.Links;

public class LinkService(IOptions<LinkOptions> options) : ILinkService
{
    public IEnumerable<Link> GetLinks() => options.Value.Links;
}