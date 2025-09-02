namespace MyBCA.Models.Bus.Responses;

public record BusAppTemplate(
    IEnumerable<BusPosition> Favorites,
    IEnumerable<BusPosition> Positions,
    DateTime? Expiry
);