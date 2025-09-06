using MyBCA.Server.Models.Links;

namespace MyBCA.Server.Services.Links;

public class LinkOptions
{
    public IEnumerable<Link> Links { get; set; } = [];
}
