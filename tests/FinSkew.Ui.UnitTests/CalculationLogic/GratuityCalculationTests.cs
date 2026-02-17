namespace FinSkew.Ui.UnitTests.CalculationLogic;

public class GratuityCalculationTests
{
    [Theory]
    [InlineData(50000, 5, 144230)]
    [InlineData(60000, 10, 346153)]
    [InlineData(80000, 20, 923076)]
    public void CalculateResult_WithEligibleYears_ReturnsExpectedOutput(
        int salary,
        int yearsOfService,
        long expectedGratuityAmount)
    {
        // Arrange
        var input = new GratuityInputViewModel
        {
            Salary = salary,
            YearsOfService = yearsOfService
        };

        // Act
        var result = CalculateGratuity(input);

        // Assert
        result.Should().NotBeNull();
        result.Inputs.Should().Be(input);
        result.GratuityAmount.Should().Be(expectedGratuityAmount);
    }

    [Theory]
    [InlineData(50000, 0)]
    [InlineData(50000, 3)]
    [InlineData(50000, 4)]
    public void CalculateResult_WithIneligibleYears_ReturnsZeroGratuity(int salary, int yearsOfService)
    {
        // Arrange
        var input = new GratuityInputViewModel
        {
            Salary = salary,
            YearsOfService = yearsOfService
        };

        // Act
        var result = CalculateGratuity(input);

        // Assert
        result.GratuityAmount.Should().Be(0);
    }

    [Fact]
    public void CalculateResult_MultipleCallsWithSameInput_ReturnsSameResult()
    {
        // Arrange
        var input = new GratuityInputViewModel
        {
            Salary = 75000,
            YearsOfService = 15
        };

        // Act
        var result1 = CalculateGratuity(input);
        var result2 = CalculateGratuity(input);

        // Assert
        result1.GratuityAmount.Should().Be(result2.GratuityAmount);
    }

    private static GratuityResultViewModel CalculateGratuity(GratuityInputViewModel input)
    {
        GratuityCalculator calculator = new();
        return calculator.Compute(input);
    }
}