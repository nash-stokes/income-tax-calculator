using API.Data.Repositories;
using API.Models.DTOs;
using API.Services.TaxCalculator;
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

    /// <summary>
    /// Computes detailed tax calculations based on the provided gross annual income.
    /// </summary>
    /// <param name="grossAnnual">The gross annual income used to calculate tax details.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="TaxResult"/> which includes the gross annual, gross monthly, annual tax, monthly tax, net annual, and net monthly amounts.</returns>
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
        var netAnnual = Math.Round(grossAnnual - annualTax, 2);
        var netMonthly = Math.Round(netAnnual / 12, 2);

        return new TaxResult(
            grossAnnual, grossMonthly,
            annualTax, monthlyTax,
            netAnnual, netMonthly);
    }
}