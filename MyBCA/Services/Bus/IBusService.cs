using MyBCA.Models.Bus;

namespace MyBCA.Services.Bus;

public interface IBusService
{
    Task<Dictionary<string, string>> GetPositionsMapAsync();
    Task<IEnumerable<BusPosition>> GetPositionsAsync();
    DateTime? Expiry { get; }
}
