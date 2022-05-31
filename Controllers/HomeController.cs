using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using sadad.Models;
using sadad.Services;

namespace sadad.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICurrencyConverter _currencyConverter;
    private readonly ISampleDataService _sampleDataService;

    public HomeController(ILogger<HomeController> logger, ICurrencyConverter currencyConverter, ISampleDataService sampleDataService)
    {
        _logger = logger;
        _currencyConverter = currencyConverter ?? throw new ArgumentNullException(nameof(currencyConverter));
        _sampleDataService = sampleDataService ?? throw new ArgumentNullException(nameof(sampleDataService));
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(ConfigViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        ViewBag.Result = _currencyConverter.Convert(model.fromCurrency, model.toCurrency, model.Amount);
        return View(model);
    }

    public IActionResult Config()
    {
        var model = _sampleDataService.ToList();
        return View(model);
    }

    [HttpPost]
    public IActionResult ClearData()
    {
        _currencyConverter.ClearConfiguration();

        return RedirectToAction(nameof(Config));
    }

    [HttpPost]
    public IActionResult ResetData()
    {
        _currencyConverter.ClearConfiguration();
        _sampleDataService.SeedData();
        return RedirectToAction(nameof(Config));
    }

    [HttpPost]
    public IActionResult Update(IEnumerable<Tuple<string, string, double>> conversionRates)
    {
        _currencyConverter.UpdateConfiguration(conversionRates);
        return RedirectToAction(nameof(Config));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
