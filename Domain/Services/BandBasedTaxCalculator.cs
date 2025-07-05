using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services;

public class BandBasedTaxCalculator : ITaxCalculator
{
    private readonly IReadOnlyList<TaxBand> _bands;

    public BandBasedTaxCalculator(IEnumerable<TaxBand> bands)
    {
        if (bands == null) throw new ArgumentNullException(nameof(bands));
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
            tax += taxableAmount * band.Rate;
        }

        return Math.Round(tax, 2);
    }
}