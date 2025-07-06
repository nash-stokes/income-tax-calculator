using API.Data.Repositories;
using API.Models.Entities;
using API.Services.TaxCalculator;
using Moq;
using Xunit;

namespace API.Tests.Services.TaxCalculator;

public class TaxCalculatorFactoryTests
{
    private readonly Mock<ITaxBandRepository> _mockRepository;
    private readonly TaxCalculatorFactory _factory;

    public TaxCalculatorFactoryTests()
    {
        _mockRepository = new Mock<ITaxBandRepository>();
        _factory = new TaxCalculatorFactory(_mockRepository.Object);
    }

    [Fact]
    public async Task CreateCalculatorAsync_ReturnsCalculatorWithBands()
    {
        // Arrange
        var bands = new List<TaxBand>
        {
            new TaxBand(0, 10000, 10 )
        };
        _mockRepository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(bands);

        // Act
        var calculator = await _factory.CreateCalculatorAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(calculator);
        Assert.IsType<BandBasedTaxCalculator>(calculator);
    }
}
