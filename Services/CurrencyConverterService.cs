using System.Linq;
using sadad.Models;

namespace sadad.Services;

public interface ICurrencyConverter
{
    /// <summary>
    /// Clears any prior configuration.
    /// </summary>
    void ClearConfiguration();
    /// <summary>
    /// Updates the configuration. Rates are inserted or replaced internally.
    /// </summary>
    void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates);

    /// <summary>
    /// Converts the specified amount to the desired currency.
    /// </summary>
    double Convert(string? fromCurrency, string? toCurrency, double amount);
}

public class CurrencyConverter : ICurrencyConverter
{
    private readonly ISampleDataService _sampleDataService;
    public CurrencyConverter(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService ?? throw new ArgumentNullException(nameof(sampleDataService));
    }
    public void ClearConfiguration()
    {
        _sampleDataService.Clear();
    }

    public double Convert(string? fromCurrency, string? toCurrency, double amount)
    {
        var config = _sampleDataService.ToList();
        var source = config.SingleOrDefault(x => x.fromCurrency == fromCurrency && x.toCurrency == toCurrency);
        if (source != null)
        {
            return Math.Round(amount * source.Rate, 2);
        }

        var fCurrency = config.SingleOrDefault(x => x.toCurrency == fromCurrency);
        var tCurrency = config.SingleOrDefault(x => x.toCurrency == toCurrency);
        if (fCurrency != null && tCurrency != null)
        {
            return Math.Round((amount / fCurrency.Rate) * tCurrency.Rate, 2);
        }
        else
        {
            return 0;
        }
    }

    public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
    {
        var data = _sampleDataService.ToList();
        foreach (var item in conversionRates)
        {
            var config = data
            .Where(x => x.fromCurrency == item.Item1)
            .Where(x => x.toCurrency == item.Item2)
            .FirstOrDefault();

            //if Exist, Update Config
            if (config != null)
            {
                config.Rate = item.Item3;
                _sampleDataService.Update(config);
            }
            
            //if Not Exist, Create New Config
            else
            {
                var newConfig = new Config() { fromCurrency = item.Item1, toCurrency = item.Item2, Rate = item.Item3 };
                _sampleDataService.Create(newConfig);
            }
        }
    }


}