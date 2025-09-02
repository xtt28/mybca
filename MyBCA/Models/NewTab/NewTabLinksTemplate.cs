using MyBCA.Models.Links;

namespace MyBCA.Models.NewTab;

public record NewTabLinksTemplate(
    IEnumerable<Link> Links
);