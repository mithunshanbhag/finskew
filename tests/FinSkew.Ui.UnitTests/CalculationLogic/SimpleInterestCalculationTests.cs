namespace FinSkew.Ui.UnitTests.CalculationLogic;

public class SimpleInterestCalculationTests
{
    [Theory]
    [InlineData(10000, 5.0, 3, 1500, 11500)]
    [InlineData(100000, 8.5, 5, 42500, 142500)]
    [InlineData(50000, 10.0, 2, 10000, 60000)]
    [InlineData(25000, 7.5, 4, 7500, 32500)]
    [InlineData(75000, 6.0, 10, 45000, 120000)]
    public void CalculateResult_WithValidInputs_ReturnsCorrectCalculation(
        int principal,
        double rate,
        int years,
        int expectedInterest,
        int expectedTotal)
    {
        // Arrange
        var input = new SimpleInterestInputViewModel
        {
            PrincipalAmount = principal,
            RateOfInterest = rate,
            TimePeriodInYears = years
        };

        // Act
        var result = CalculateSimpleInterest(input);

        // Assert
        result.Should().NotBeNull();
        result.Inputs.Should().Be(input);
        result.TotalInterestEarned.Should().Be(expectedInterest);
        result.TotalAmount.Should().Be(expectedTotal);
    }

    [Fact]
    public void CalculateResult_WithMinimumValidInput_ReturnsCorrectResult()
    {
        // Arrange
        var input = new SimpleInterestInputViewModel
        {
            PrincipalAmount = 10000,
            RateOfInterest = 1.0,
            TimePeriodInYears = 1
        };

        // Act
        var result = CalculateSimpleInterest(input);

        // Assert
        result.TotalInterestEarned.Should().Be(100);
        result.TotalAmount.Should().Be(10100);
    }

    [Fact]
    public void CalculateResult_WithMaximumValidInput_ReturnsCorrectResult()
    {
        // Arrange
        var input = new SimpleInterestInputViewModel
        {
            PrincipalAmount = 10000000,
            RateOfInterest = 15.0,
            TimePeriodInYears = 10
        };

        // Act
        var result = CalculateSimpleInterest(input);

        // Assert
        result.TotalInterestEarned.Should().Be(15000000);
        result.TotalAmount.Should().Be(25000000);
    }

    [Theory]
    [InlineData(15000, 4.5, 7)]
    [InlineData(80000, 9.25, 15)]
    [InlineData(120000, 5.75, 20)]
    public void CalculateResult_WithDecimalRates_RoundsCorrectly(int principal, double rate, int years)
    {
        // Arrange
        var input = new SimpleInterestInputViewModel
        {
            PrincipalAmount = principal,
            RateOfInterest = rate,
            TimePeriodInYears = years
        };

        // Act
        var result = CalculateSimpleInterest(input);

        // Assert
        result.TotalInterestEarned.Should().BeGreaterThan(0);
        result.TotalAmount.Should().Be(result.Inputs.PrincipalAmount + result.TotalInterestEarned);
    }

    [Fact]
    public void CalculateResult_ResultsConsistency_TotalAmountEqualsPrincipalPlusInterest()
    {
        // Arrange
        var faker = new Faker();
        var input = new SimpleInterestInputViewModel
        {
            PrincipalAmount = faker.Random.Int(10000, 100000),
            RateOfInterest = faker.Random.Double(1.0, 15.0),
            TimePeriodInYears = faker.Random.Int(1, 30)
        };

        // Act
        var result = CalculateSimpleInterest(input);

        // Assert
        result.TotalAmount.Should().Be(result.Inputs.PrincipalAmount + result.TotalInterestEarned);
    }

    [Theory]
    [InlineData(10000, 5.0, 0)]
    [InlineData(10000, 5.0, -1)]
    public void CalculateResult_WithZeroOrNegativeYears_ReturnsZeroOrNegativeInterest(int principal, double rate,
        int years)
    {
        // Arrange
        var input = new SimpleInterestInputViewModel
        {
            PrincipalAmount = principal,
            RateOfInterest = rate,
            TimePeriodInYears = years
        };

        // Act
        var result = CalculateSimpleInterest(input);

        // Assert
        if (years == 0)
        {
            result.TotalInterestEarned.Should().Be(0);
            result.TotalAmount.Should().Be(principal);
        }
        else
        {
            result.TotalInterestEarned.Should().BeLessThan(0);
        }
    }

    // Helper method that mimics the calculator logic
    private static SimpleInterestResultViewModel CalculateSimpleInterest(SimpleInterestInputViewModel input)
    {
        SimpleInterestCalculator calculator = new();
        return calculator.Compute(input);
    }
}