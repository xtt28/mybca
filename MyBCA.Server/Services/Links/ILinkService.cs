using MyBCA.Server.Models.Links;

namespace MyBCA.Server.Services.Links;

public interface ILinkService
{
    IEnumerable<Link> GetLinks();
}