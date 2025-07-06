using API.Data.Repositories;
using API.Models.Entities;
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
        private readonly Mock<IMemoryCache> _mockMemoryCache;

        public TaxServiceTests()
        {
            var taxBands = new List<TaxBand> { new TaxBand(0, 5000, 0), new TaxBand(5000,20000,20), new TaxBand(20000, null, 40) };
            var mockRepository = new Mock<ITaxBandRepository>();
            _mockMemoryCache = new Mock<IMemoryCache>();
            _mockCalculator = new Mock<ITaxCalculator>();
            var _mockCacheEntry = new Mock<ICacheEntry>();
            
            mockRepository
                .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(taxBands);
        
            _mockMemoryCache
                .Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
                .Returns(false);  // Cache miss
        
            _mockMemoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(_mockCacheEntry.Object);

            _service = new TaxService(
                mockRepository.Object,
                _mockMemoryCache.Object,
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
            Assert.Equal(Math.Round(salary / 12, 2), result.GrossMonthly);
            Assert.Equal(expectedTax, result.AnnualTax);
            Assert.Equal(Math.Round(expectedTax / 12, 2), result.MonthlyTax);
            Assert.Equal(Math.Round(salary - expectedTax, 2), result.NetAnnual);
            Assert.Equal(Math.Round((salary - expectedTax) / 12, 2), result.NetMonthly);
        }

        [Fact]
        public async Task ComputeAsync_WithZeroSalary_ReturnsZeroTax()
        {
            // Arrange
            var bands = new List<TaxBand> { new TaxBand(0, 50000, 20) };
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
        
        [Fact]
        public async Task ComputeAsync_CacheMiss_CallsRepository()
        {
            // Arrange
            var mockRepository = new Mock<ITaxBandRepository>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var mockCacheEntry = new Mock<ICacheEntry>();
            var mockCalculator = new Mock<ITaxCalculator>();
            var taxBands = new List<TaxBand> { new TaxBand(0, 50000, 20) };
    
            mockRepository
                .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(taxBands);
        
            _mockMemoryCache
                .Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
                .Returns(false);  // Cache miss
        
            mockMemoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(mockCacheEntry.Object);
        
            mockCalculator
                .Setup(x => x.CalculateAnnualTax(It.IsAny<decimal>(), It.IsAny<CancellationToken>()))
                .Returns(10000m);
        
            var service = new TaxService(
                mockRepository.Object,
                mockMemoryCache.Object,
                mockCalculator.Object);
        
            // Act
            await service.ComputeAsync(50000m, CancellationToken.None);
    
            // Assert
            mockRepository.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
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