namespace API.Services;

public interface ITaxCalculatorFactory
{
    Task<ITaxCalculator> CreateCalculatorAsync(CancellationToken ct);
}
