using MyBCA.Shared.Models.Links;

namespace MyBCA.Server.Services.Links;

public interface ILinkService
{
    IEnumerable<Link> GetLinks();
}