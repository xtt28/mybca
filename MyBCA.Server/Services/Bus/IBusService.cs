using MyBCA.Shared.Models.Bus;

namespace MyBCA.Server.Services.Bus;

public interface IBusService
{
    Task<Dictionary<string, string>> GetPositionsMapAsync();
    Task<IEnumerable<BusPosition>> GetPositionsAsync();
    DateTime? Expiry { get; }
}
