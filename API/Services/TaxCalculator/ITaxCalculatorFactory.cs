namespace API.Services.TaxCalculator;

public interface ITaxCalculatorFactory
{
    Task<ITaxCalculator> CreateCalculatorAsync(CancellationToken ct);
}
