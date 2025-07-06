using API.Data.Repositories;
using API.Models.DTOs;
using Microsoft.Extensions.Caching.Memory;

namespace API.Services;

public class TaxService : ITaxService
{
    private const string BandsCacheKey = "TaxBands";
    private readonly IMemoryCache _cache;
    private readonly ITaxCalculator _calculator;
    private readonly ITaxBandRepository _repo;

    public TaxService(
        ITaxBandRepository repo,
        IMemoryCache cache,
        ITaxCalculator calculator)
    {
        _repo = repo;
        _cache = cache;
        _calculator = calculator;
    }

    public async Task<TaxResult> ComputeAsync(decimal grossAnnual, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var bands = await _cache.GetOrCreateAsync(BandsCacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            return await _repo.GetAllAsync(ct);
        });

        var annualTax = _calculator.CalculateAnnualTax(grossAnnual, ct);
        var grossMonthly = Math.Round(grossAnnual / 12, 2);
        var monthlyTax = Math.Round(annualTax / 12, 2);
        var netAnnual = grossAnnual - annualTax;
        var netMonthly = grossMonthly - monthlyTax;

        return new TaxResult(
            grossAnnual, grossMonthly,
            annualTax, monthlyTax,
            netAnnual, netMonthly);
    }
}