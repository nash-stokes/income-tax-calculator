namespace Domain.Entities;

public class TaxBand
{
    // Required by EF Core
    private TaxBand()
    {
    }

    public TaxBand(int lowerLimit, int? upperLimit, int rate)
    {
        if (lowerLimit < 0) throw new ArgumentException(nameof(lowerLimit));
        if (rate is < 0 or > 1) throw new ArgumentException(nameof(rate));

        LowerLimit = lowerLimit;
        if (upperLimit != null)
        {
            UpperLimit = upperLimit;
        }
        Rate = rate;
    }

    public int Id { get; private set; }
    public int LowerLimit { get; private set; }
    public int? UpperLimit { get; private set; }
    public int Rate { get; private set; }
}