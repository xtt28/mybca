using System.Text.Json;
using Microsoft.Extensions.Options;
using MyBCA.Models;

namespace MyBCA.Services.Nutrislice
{
    public class NutrisliceService(HttpClient httpClient, IOptions<NutrisliceOptions> options) : INutrisliceService
    {
        public DateTime? Expiry { get; private set; } = null;
        private MenuWeek? _cachedWeekData;

        private readonly HttpClient _httpClient = httpClient;
        private readonly NutrisliceOptions _options = options.Value;

        public async Task<MenuWeek> GetMenuWeekAsync()
        {
            var now = DateTime.Now;

            if (now < Expiry && _cachedWeekData != null)
            {
                return _cachedWeekData;
            }

            try
            {
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    PropertyNameCaseInsensitive = true
                };
                var response = await _httpClient.GetFromJsonAsync<MenuWeek>($"{now.Year}/{now.Month}/{now.Day}", jsonOptions)
                    ?? throw new InvalidOperationException("Received empty response from Nutrislice API");

                _cachedWeekData = response;
                Expiry = now.Add(_options.CacheTTL);

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
}