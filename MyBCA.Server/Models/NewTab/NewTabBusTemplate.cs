using MyBCA.Server.Models.Bus;

namespace MyBCA.Server.Models.NewTab;

public record NewTabBusTemplate(
    BusPosition Position,
    DateTime? Expiry
);