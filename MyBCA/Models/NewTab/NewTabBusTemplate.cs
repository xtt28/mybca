using MyBCA.Models.Bus;

namespace MyBCA.Models.NewTab;

public record NewTabBusTemplate(
    BusPosition Position,
    DateTime? Expiry
);