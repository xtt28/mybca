using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MyBCA.Models;
using MyBCA.Models.Nutrislice;

namespace MyBCA.Services.Nutrislice;

public class NutrisliceService(HttpClient httpClient, IOptions<NutrisliceOptions> options, IMemoryCache cache) : INutrisliceService
{
    private const string CacheKey = "MenuWeek";

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
    private readonly NutrisliceOptions _options = options.Value;

    public async Task<MenuWeek> GetMenuWeekAsync()
    {
        if (_cache.TryGetValue<CacheItem<MenuWeek>>(CacheKey, out var cachedWeek))
        {
            return cachedWeek!.Value;
        }

        try
        {
            var now = DateTime.Now;

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                PropertyNameCaseInsensitive = true
            };
            var response = await _httpClient.GetFromJsonAsync<MenuWeek>($"{now.Year}/{now.Month}/{now.Day}", jsonOptions)
                ?? throw new InvalidOperationException("Received empty response from Nutrislice API");

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(options.Value.CacheTtl);
            _cache.Set(CacheKey, new CacheItem<MenuWeek>
            {
                Value = response,
                Expiry = DateTime.Now + options.Value.CacheTtl
            }, cacheEntryOptions);

            return response;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Could not connect to Nutrislice API.", ex);
        }
        catch (NotSupportedException ex)
        {
            throw new Exception("Received unsupported content type from Nutrislice.", ex);
        }
        catch (JsonException ex)
        {
            throw new Exception("Could not deserialize Nutrislice JSON response.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Could not fetch menu week data.", ex);
        }
    }

    public async Task<MenuDay?> GetMenuDayAsync()
    {
        var weekData = await GetMenuWeekAsync();
        var todayWeekday = (int)DateTime.Now.DayOfWeek;

        return weekData.Days.ElementAtOrDefault(todayWeekday);
    }
}
