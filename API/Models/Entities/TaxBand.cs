namespace API.Models.Entities;

public class TaxBand
{
    // Required by EF Core
    private TaxBand()
    {
    }

    public TaxBand(int lowerLimit, int? upperLimit, int rate)
    {
        if (lowerLimit < 0) throw new ArgumentException("Invalid lower limit. Must be greater than or equal to zero.");
        if (rate is < 0 or > 100) throw new ArgumentException("Invalid tax rate. Must be between 0 and 100 inclusive.");

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