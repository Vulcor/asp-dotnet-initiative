using Microsoft.AspNetCore.Mvc;

public class ExchangeMenuViewComponent : ViewComponent
{
    private readonly ExchangeRateService _rateService;
    private readonly ILogger<ExchangeMenuViewComponent> _logger;

    public ExchangeMenuViewComponent(ExchangeRateService rateService, ILogger<ExchangeMenuViewComponent> logger)
    {
        _rateService = rateService;
        _logger = logger;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // plan was to give user an option to manage pairs manually, but I didn't have time to implement it
        var defaultPairs = new[] {("USD", "AUD"), ("USD", "EUR"), ("EUR", "AUD")};

        try
        {
            var rates = await _rateService.GetConversionRatesAsync(defaultPairs);
            return View(rates ?? new Dictionary<string, double?>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve exchange rates.");
            return View(new Dictionary<string, double?>());
        }
    }
}
