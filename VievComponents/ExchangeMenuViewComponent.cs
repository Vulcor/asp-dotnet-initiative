using Microsoft.AspNetCore.Mvc;

public class ExchangeMenuViewComponent : ViewComponent
{
    private readonly ExchangeRateService _rateService;

    public ExchangeMenuViewComponent(ExchangeRateService rateService)
    {
        _rateService = rateService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // plan was to give user an option to manage pairs manually, but I didn't have time to implement it
        var defaultPairs = new[] {("USD", "AUD"), ("USD", "EUR"), ("EUR", "AUD")};

        var rates = await _rateService.GetConversionRatesAsync(defaultPairs);
        return View(rates);
    }
}
