using MyBCA.Server.Models.Bus;

namespace MyBCA.Server.Models.NewTab;

public record NewTabTemplate(
    NewTabBusTemplate? BusData,
    NewTabLinksTemplate LinksData,
    NewTabLunchTemplate? LunchData,
    IEnumerable<string>? TownList
);