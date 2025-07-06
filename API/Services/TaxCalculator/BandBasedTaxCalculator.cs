using API.Models.Entities;

namespace API.Services.TaxCalculator;

public class BandBasedTaxCalculator : ITaxCalculator
{
    private readonly IReadOnlyList<TaxBand> _bands;

    public BandBasedTaxCalculator(IEnumerable<TaxBand> bands)
    {
        ArgumentNullException.ThrowIfNull(bands);
        // Ensure bands are sorted by LowerLimit
        _bands = bands.OrderBy(b => b.LowerLimit).ToList();
    }

    public decimal CalculateAnnualTax(decimal grossAnnual, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        decimal tax = 0;
        foreach (var band in _bands)
        {
            ct.ThrowIfCancellationRequested();

            if (grossAnnual <= band.LowerLimit)
                break;

            // Determine the top of this band (or the salary if no upper limit)
            var upper = band.UpperLimit ?? grossAnnual;
            // Compute how much falls into this band
            var taxableAmount = Math.Min(grossAnnual, upper) - band.LowerLimit;
            tax += (taxableAmount * band.Rate) / 100;
        }

        return Math.Round(tax, 2);
    }
}