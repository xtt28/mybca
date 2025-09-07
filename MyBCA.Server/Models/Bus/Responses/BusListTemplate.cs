namespace MyBCA.Server.Models.Bus.Responses;

using MyBCA.Shared.Models.Bus;

public record BusListTemplate(
    IEnumerable<BusPosition> Favorites,
    IEnumerable<BusPosition> Positions,
    DateTime? Expiry
);