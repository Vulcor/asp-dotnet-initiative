using System.Text.Json;
using Microsoft.Extensions.Options;

public class ExchangeRateService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl;
    private readonly string _apiKey;

    private Dictionary<string, double?> _cachedRates = new();
    private DateTime _lastFetchTime = DateTime.MinValue;

    public ExchangeRateService(HttpClient httpClient, IOptions<ExchangeRateApiOptions> options)
    {
        _httpClient = httpClient;
        _apiUrl = options.Value.BaseUrl;
        _apiKey = options.Value.AccessKey;
    }

    public async Task<Dictionary<string, double?>> GetConversionRatesAsync((string from, string to)[] pairs)
    {
        if ((DateTime.UtcNow - _lastFetchTime) < TimeSpan.FromMinutes(10))
        {
            return _cachedRates;
        }

        string url = $"{_apiUrl}?access_key={_apiKey}";
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return _cachedRates;

        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content);
        var quotes = json.RootElement.GetProperty("quotes");

        var result = new Dictionary<string, double?>();

        foreach (var (from, to) in pairs)
        {
            string key = $"{from}/{to}";

            if (from == "USD" && quotes.TryGetProperty("USD" + to, out var direct))
            {
                result[key] = direct.GetDouble();
            }
            else if (to == "USD" && quotes.TryGetProperty("USD" + from, out var reverse))
            {
                result[key] = 1.0 / reverse.GetDouble();
            }
            else if (quotes.TryGetProperty("USD" + from, out var fromRate) &&
                     quotes.TryGetProperty("USD" + to, out var toRate))
            {
                result[key] = toRate.GetDouble() / fromRate.GetDouble();
            }
            else
            {
                result[key] = null;
            }
        }

        _cachedRates = result;
        _lastFetchTime = DateTime.UtcNow;

        return result;
    }
}