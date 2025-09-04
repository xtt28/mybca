using MyBCA.Models.Bus;

namespace MyBCA.Models.NewTab;

public record NewTabTemplate(
    NewTabBusTemplate? BusData,
    NewTabLinksTemplate LinksData,
    NewTabLunchTemplate? LunchData
);