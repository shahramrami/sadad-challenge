using Microsoft.EntityFrameworkCore;
using sadad.Data;
using sadad.Models;

namespace sadad.Services;

public interface ISampleDataService
{
    void SeedData();
    IList<Config> ToList();
    Config Find(int id);
    void Create(Config model);
    void Update(Config model);
    void Remove(Config model);
    void Clear();
}

public class SampleDataService : ISampleDataService
{
    private readonly AppDbContext _context;
    private readonly DbSet<Config> _config;

    public SampleDataService(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _config = _context.Set<Config>();
    }

    public void SeedData()
    {
        var c1 = new Config() { fromCurrency = "USD", toCurrency = "USD", Rate = 1.0 };
        var c2 = new Config() { fromCurrency = "USD", toCurrency = "CAD", Rate = 1.34 };
        var c4 = new Config() { fromCurrency = "USD", toCurrency = "EUR", Rate = 0.86 };
        var c3 = new Config() { fromCurrency = "USD", toCurrency = "GBR", Rate = 0.58 };
        _config.Add(c1);
        _config.Add(c2);
        _config.Add(c3);
        _config.Add(c4);
        _context.SaveChanges();
    }

    public IList<Config> ToList()
    {
        return _config.OrderBy(x => x.Id).ToList();
    }

    public Config Find(int id)
    {
        return _config.Find(id);
    }

    public void Create(Config model)
    {
        _config.Add(model);
        _context.SaveChanges();
    }

    public void Update(Config model)
    {
        _config.Update(model);
        _context.SaveChanges();
    }

    public void Remove(Config model)
    {
        _config.Remove(model);
        _context.SaveChanges();
    }

    public void Clear()
    {
        _config.RemoveRange(_config.ToList());
        _context.SaveChanges();
    }
}