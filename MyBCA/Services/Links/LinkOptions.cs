using MyBCA.Models.Links;

namespace MyBCA.Services.Links;

public class LinkOptions
{
    public IEnumerable<Link> Links { get; set; } = [];
}
