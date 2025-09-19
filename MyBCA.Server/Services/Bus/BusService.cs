using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MyBCA.Server.Models;
using MyBCA.Shared.Models.Bus;

namespace MyBCA.Server.Services.Bus;

public class BusService(ILogger<BusService> logger, HttpClient httpClient, IOptions<BusOptions> options, IMemoryCache cache) : IBusService
{
    private const string CacheKey = "BusPositionMap";

    public DateTime? Expiry
    {
        get
        {
            if (cache.TryGetValue<CacheItem<Dictionary<string, string>>>(CacheKey, out var cachedPositions))
            {
                return cachedPositions!.Expiry;
            }

            return null;
        }
    }

    public string? SourceUrl => options.Value.BaseUrl;

    private static bool IsBetween(TimeSpan time, TimeSpan lower, TimeSpan upper) => time >= lower && time <= upper;

    private TimeSpan GetCacheTtl(DateTime now)
    {
        var nowTime = now.TimeOfDay;
        if (IsBetween(nowTime, new TimeSpan(12, 25, 0), new TimeSpan(12, 50, 0))
            || IsBetween(nowTime, new TimeSpan(16, 5, 0), new TimeSpan(16, 30, 0)))
        {
            return options.Value.CacheTtlDismissalTime;
        }

        return options.Value.CacheTtlNormal;
    }

    public async Task<Dictionary<string, string>> GetPositionsMapAsync()
    {
        if (cache.TryGetValue<CacheItem<Dictionary<string, string>>>(CacheKey, out var cachedPositions))
        {
            logger.LogDebug("Using cached bus position data");
            return cachedPositions!.Value;
        }

        try
        {
            logger.LogInformation("Fetching new bus position data");
            var html = await httpClient.GetStringAsync("");
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var positionMap = BusSheetReader.ParseTableToPositionMap(doc);

            var now = DateTime.Now;
            var ttl = GetCacheTtl(now);
            logger.LogInformation("Cache TTL for newly fetched bus position data is {Ttl}", ttl);

            if (positionMap.Count < 20)
            {
                logger.LogWarning("Only {Count} bus positions found - that's weird!", positionMap.Count);
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(ttl);
            cache.Set(CacheKey, new CacheItem<Dictionary<string, string>>
            {
                Value = positionMap,
                Expiry = DateTime.Now + ttl
            }, cacheEntryOptions);

            return positionMap;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Could not connect to bus sheet.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Could not fetch bus data.", ex);
        }
    }

    public async Task<IEnumerable<BusPosition>> GetPositionsAsync()
    {
        var positionsMap = await GetPositionsMapAsync();
        var positions = new List<BusPosition>(positionsMap.Count);

        foreach (var (town, location) in positionsMap)
        {
            positions.Add(new BusPosition(town, location));
        }

        return positions;
    }

    public async Task<IEnumerable<string>> GetBusNamesAsync()
    {
        var positionsMap = await GetPositionsMapAsync();
        var mapKeys = positionsMap.Keys.ToList();
        mapKeys.Sort();

        return mapKeys;
    }
}
