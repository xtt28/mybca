using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MyBCA.Server.Models;
using MyBCA.Server.Models.Bus;

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

            // In Google Sheets, main sheet table is located in table element with waffle class
            var table = doc.DocumentNode.SelectSingleNode("//table[contains(@class, 'waffle')]")
                ?? throw new InvalidDataException("Table not found on page");

            // Skip first row (header)
            var rows = table.SelectNodes("tbody/tr").Cast<HtmlNode>().Skip(1);
            var positionMap = new Dictionary<string, string>();

            foreach (var row in rows)
            {
                var cells = row.SelectNodes("td").Cast<HtmlNode>();
                for (int i = 0; i < 4; i += 2)
                {
                    var cellContent = cells.ElementAt(i).InnerText;
                    if (string.IsNullOrWhiteSpace(cellContent))
                    {
                        continue;
                    }

                    positionMap[cellContent] = cells.ElementAt(i + 1).InnerText;
                }
            }

            var now = DateTime.Now;
            var ttl = GetCacheTtl(DateTime.Now);
            logger.LogInformation("Cache TTL for newly fetched bus position data is {Ttl}", ttl);

            if (positionMap.Count < 20)
            {
                logger.LogWarning("Less than {Count} bus positions found - that's weird!", positionMap.Count);
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
}
