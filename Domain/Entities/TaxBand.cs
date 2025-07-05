namespace Domain.Entities;

public class TaxBand
{
    // Required by EF Core
    private TaxBand()
    {
    }

    public TaxBand(decimal lowerLimit, decimal? upperLimit, decimal rate)
    {
        if (lowerLimit < 0) throw new ArgumentException(nameof(lowerLimit));
        if (rate is < 0 or > 1) throw new ArgumentException(nameof(rate));

        LowerLimit = lowerLimit;
        UpperLimit = upperLimit;
        Rate = rate;
    }

    public int Id { get; private set; }
    public decimal LowerLimit { get; private set; }
    public decimal? UpperLimit { get; private set; }
    public decimal Rate { get; private set; }
}