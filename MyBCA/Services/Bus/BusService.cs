using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MyBCA.Models;
using MyBCA.Models.Bus;

namespace MyBCA.Services.Bus;

public class BusService(HttpClient httpClient, IOptions<BusOptions> options, IMemoryCache cache) : IBusService
{
    private const string CacheKey = "BusPositionMap";

    public DateTime? Expiry
    {
        get
        {
            if (_cache.TryGetValue<CacheItem<Dictionary<string, string>>>(CacheKey, out var cachedPositions))
            {
                return cachedPositions!.Expiry;
            }

            return null;
        }
    }

    private readonly IMemoryCache _cache = cache;
    private readonly HttpClient _httpClient = httpClient;
    private readonly BusOptions _options = options.Value;

    private static bool IsBetween(TimeSpan time, TimeSpan lower, TimeSpan upper) => time >= lower && time <= upper;

    private TimeSpan GetCacheTtl(DateTime now)
    {
        var nowTime = now.TimeOfDay;
        if (IsBetween(nowTime, new TimeSpan(12, 25, 0), new TimeSpan(12, 50, 0))
            || IsBetween(nowTime, new TimeSpan(16, 5, 0), new TimeSpan(16, 30, 0)))
        {
            return _options.CacheTtlDismissalTime;
        }

        return _options.CacheTtlNormal;
    }

    public async Task<Dictionary<string, string>> GetPositionsMapAsync()
    {
        if (_cache.TryGetValue<CacheItem<Dictionary<string, string>>>(CacheKey, out var cachedPositions))
        {
            return cachedPositions!.Value;
        }

        try
        {
            var html = await _httpClient.GetStringAsync("");
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
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(ttl);
            _cache.Set(CacheKey, new CacheItem<Dictionary<string, string>>
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
