using API.Data.Repositories;

namespace API.Services.TaxCalculator;

public class TaxCalculatorFactory(ITaxBandRepository repository) : ITaxCalculatorFactory
{
    public async Task<ITaxCalculator> CreateCalculatorAsync(CancellationToken ct)
    {
        var taxBands = await repository.GetAllAsync(ct);
        return new BandBasedTaxCalculator(taxBands);
    }
}
