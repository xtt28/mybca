using MyBCA.Shared.Models.Links;

namespace MyBCA.Server.Models.NewTab;

public record NewTabLinksTemplate(
    IEnumerable<Link> Links
);