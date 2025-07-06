using API.Controllers;
using API.Models.DTOs;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers;

public class TaxControllerTests
{
    private readonly Mock<ITaxService> _mockTaxService;
    private readonly TaxController _controller;

    public TaxControllerTests()
    {
        _mockTaxService = new Mock<ITaxService>();
        _controller = new TaxController(_mockTaxService.Object);
    }

    [Fact]
    public async Task GetTax_WithNegativeSalary_ReturnsBadRequest()
    {
        // Arrange
        decimal salary = -1000;

        // Act
        var result = await _controller.GetTax(salary, CancellationToken.None);

        // Assert
        var validationProblemDetails = Assert.IsType<BadRequestObjectResult>(result); 
            
        Assert.Equal(400, validationProblemDetails.StatusCode); // Validate the status code from ProblemDetails
    }

    [Fact]
    public async Task GetTax_WithValidSalary_ReturnsOkResult()
    {
        // Arrange
        decimal salary = 50000;
        var expectedResult = new TaxResult(50000, 4166.67m, 10000, 833.33m, 40000, 3333.34m);
        _mockTaxService.Setup(x => x.ComputeAsync(salary, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetTax(salary, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var taxResult = Assert.IsType<TaxResult>(okResult.Value);
        Assert.Equal(expectedResult, taxResult);
    }
}
