namespace MyBCA.Server.Models.Bus.Responses;

using MyBCA.Shared.Models.Bus;

public record BusInspectorTemplate(
    IEnumerable<BusPosition> Positions,
    DateTime? Expiry,
    string? SourceUrl
);