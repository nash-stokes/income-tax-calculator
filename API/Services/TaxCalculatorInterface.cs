namespace Domain.Interfaces;

public interface ITaxCalculatorFactory
{
    Task<ITaxCalculator> CreateCalculatorAsync(CancellationToken ct);
}
