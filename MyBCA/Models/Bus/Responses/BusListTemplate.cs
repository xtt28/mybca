namespace MyBCA.Models.Bus.Responses;

public record BusListTemplate(
    IEnumerable<BusPosition> Favorites,
    IEnumerable<BusPosition> Positions,
    DateTime? Expiry
);