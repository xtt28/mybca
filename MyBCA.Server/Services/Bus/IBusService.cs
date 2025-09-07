using MyBCA.Shared.Models.Bus;

namespace MyBCA.Server.Services.Bus;

public interface IBusService
{
    Task<Dictionary<string, string>> GetPositionsMapAsync();
    Task<IEnumerable<BusPosition>> GetPositionsAsync();
    Task<IEnumerable<string>> GetBusNamesAsync();
    string? SourceUrl { get; }
    DateTime? Expiry { get; }
}
