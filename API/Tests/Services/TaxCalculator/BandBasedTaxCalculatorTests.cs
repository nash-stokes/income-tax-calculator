using API.Models.Entities;
using API.Services.TaxCalculator;
using Xunit;

namespace API.Tests.Services.TaxCalculator;

public class BandBasedTaxCalculatorTests
{
    [Fact]
    public void Constructor_WithNullBands_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new BandBasedTaxCalculator(null));
    }

    [Fact]
    public void CalculateAnnualTax_WithSimpleBands_CalculatesCorrectly()
    {
        // Arrange
        var bands = new List<TaxBand>
        {
            new TaxBand(0, 10000, 10 ),
            new TaxBand ( 10000, 50000, 20 ),
            new TaxBand (50000, null, 40 )
        };
        var calculator = new BandBasedTaxCalculator(bands);

        // Act
        var tax = calculator.CalculateAnnualTax(30000, CancellationToken.None);

        // Assert
        // Expected: (10000 * 0.10) + (20000 * 0.20) = 1000 + 4000 = 5000
        Assert.Equal(5000m, tax);
    }

    [Fact]
    public void CalculateAnnualTax_WithCancellation_ThrowsOperationCanceledException()
    {
        // Arrange
        var bands = new List<TaxBand>
        {
            new TaxBand(0, 10000, 10 )
        };
        var calculator = new BandBasedTaxCalculator(bands);
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        Assert.Throws<OperationCanceledException>(() => 
            calculator.CalculateAnnualTax(5000, cts.Token));
    }
}

