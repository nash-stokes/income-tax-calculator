using API.Data.Repositories;

namespace API.Services;

public class TaxCalculatorFactory : ITaxCalculatorFactory
{
    private readonly ITaxBandRepository _repository;

    public TaxCalculatorFactory(ITaxBandRepository repository)
    {
        _repository = repository;
    }

    public async Task<ITaxCalculator> CreateCalculatorAsync(CancellationToken ct)
    {
        var taxBands = await _repository.GetAllAsync(ct);
        return new BandBasedTaxCalculator(taxBands);
    }
}
