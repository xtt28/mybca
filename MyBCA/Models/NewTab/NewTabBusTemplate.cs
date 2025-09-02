using MyBCA.Models.Bus;

namespace MyBCA.Models.NewTab;

public record NewTabBusTemplate(
    IEnumerable<BusPosition> Positions,
    DateTime Expiry
);