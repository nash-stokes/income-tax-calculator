namespace API.Services;

public interface ITaxCalculator
{
    /// <summary>
    ///     Calculates annual tax on the given salary.
    /// </summary>
    /// <param name="grossAnnual">Gross annual salary.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Annual tax amount.</returns>
    decimal CalculateAnnualTax(decimal grossAnnual, CancellationToken ct);
}