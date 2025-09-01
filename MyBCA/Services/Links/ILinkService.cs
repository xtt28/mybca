using MyBCA.Models.Links;

namespace MyBCA.Services.Links;

public interface ILinkService
{
    IEnumerable<Link> GetLinks();
}