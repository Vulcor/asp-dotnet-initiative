using System.Text.Json;
using Microsoft.Extensions.Options;

public class ExchangeRateService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl;
    private readonly string _apiKey;

    public ExchangeRateService(HttpClient httpClient, IOptions<ExchangeRateApiOptions> options)
    {
        _httpClient = httpClient;
        _apiUrl = options.Value.BaseUrl;
        _apiKey = options.Value.AccessKey;
    }

    public async Task<Dictionary<string, double?>> GetConversionRatesAsync((string from, string to)[] pairs)
    {
        string url = $"{_apiUrl}?access_key={_apiKey}";
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return null!;

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

        return result;
    }
}