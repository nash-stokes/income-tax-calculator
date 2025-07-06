using API.Data.Repositories;
using API.Services;
using API.Services.TaxCalculator;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace API.Tests.Services
{
    public class TaxServiceTests
    {
        private readonly Mock<ITaxCalculator> _mockCalculator;
        private readonly TaxService _service;

        public TaxServiceTests()
        {
            var mockRepository = new Mock<ITaxBandRepository>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            _mockCalculator = new Mock<ITaxCalculator>();

            _service = new TaxService(
                mockRepository.Object,
                mockMemoryCache.Object,
                _mockCalculator.Object);
        }



        [Theory]
        [InlineData(50000, 15000)]
        [InlineData(40000, 11000)]
        [InlineData(10000, 1000)]
        public async Task ComputeAsync_WithValidSalary_ReturnsTaxResult(decimal salary, decimal expectedTax)
        {
            // Arrange
            _mockCalculator
                .Setup(x => x.CalculateAnnualTax(salary, It.IsAny<CancellationToken>()))
                .Returns(expectedTax);

            // Act
            var result = await _service.ComputeAsync(salary, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(salary, result.GrossAnnual);
            Assert.Equal(salary / 12, result.GrossMonthly);
            Assert.Equal(expectedTax, result.AnnualTax);
            Assert.Equal(expectedTax / 12, result.MonthlyTax);
            Assert.Equal(salary - expectedTax, result.NetAnnual);
            Assert.Equal((salary - expectedTax) / 12, result.NetMonthly);
        }

        [Fact]
        public async Task ComputeAsync_WithZeroSalary_ReturnsZeroTax()
        {
            // Arrange
            decimal salary = 0;
            _mockCalculator
                .Setup(x => x.CalculateAnnualTax(salary, It.IsAny<CancellationToken>()))
                .Returns(0);

            // Act
            var result = await _service.ComputeAsync(salary, CancellationToken.None);

            // Assert
            Assert.Equal(0, result.AnnualTax);
            Assert.Equal(0, result.MonthlyTax);
            Assert.Equal(0, result.NetAnnual);
            Assert.Equal(0, result.NetMonthly);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-1000)]
        public async Task ComputeAsync_WithNegativeSalary_ThrowsArgumentException(decimal salary)
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _service.ComputeAsync(salary, CancellationToken.None));
        }
    }
}