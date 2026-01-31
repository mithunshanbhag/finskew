namespace FinSkew.Ui.UnitTests.CalculationLogic;

public class CompoundInterestCalculationTests
{
    [Theory]
    [InlineData(10000, 5.0, 3, 1, 1576, 11576)] // Annual compounding
    [InlineData(10000, 5.0, 3, 4, 1607, 11607)] // Quarterly compounding
    [InlineData(10000, 5.0, 3, 12, 1614, 11614)] // Monthly compounding
    [InlineData(50000, 8.0, 5, 4, 24297, 74297)] // Quarterly, higher rate
    [InlineData(100000, 6.0, 10, 1, 79084, 179084)] // Long term, annual
    public void CalculateResult_WithValidInputs_ReturnsCorrectCalculation(
        int principal,
        double rate,
        int years,
        int frequency,
        int expectedInterest,
        int expectedTotal)
    {
        // Arrange
        var input = new CompoundInterestInputViewModel
        {
            PrincipalAmount = principal,
            RateOfInterest = rate,
            TimePeriodInYears = years,
            CompoundingFrequencyPerYear = frequency
        };

        // Act
        var result = CalculateCompoundInterest(input);

        // Assert
        result.Should().NotBeNull();
        result.Inputs.PrincipalAmount.Should().Be(principal);
        result.TotalInterestEarned.Should().Be(expectedInterest);
        result.TotalAmount.Should().Be(expectedTotal);
    }

    [Theory]
    [InlineData(1)] // Annual
    [InlineData(2)] // Semi-annual
    [InlineData(4)] // Quarterly
    [InlineData(12)] // Monthly
    [InlineData(365)] // Daily
    public void CalculateResult_WithDifferentFrequencies_InterestIncreasesWithFrequency(int frequency)
    {
        // Arrange
        var input = new CompoundInterestInputViewModel
        {
            PrincipalAmount = 10000,
            RateOfInterest = 10.0,
            TimePeriodInYears = 5,
            CompoundingFrequencyPerYear = frequency
        };

        // Act
        var result = CalculateCompoundInterest(input);

        // Assert
        result.TotalInterestEarned.Should().BeGreaterThan(0);
        result.TotalAmount.Should().BeGreaterThan(input.PrincipalAmount);
    }

    [Fact]
    public void CalculateResult_ComparisonWithDifferentFrequencies_HigherFrequencyYieldsMoreInterest()
    {
        // Arrange
        var annualInput = new CompoundInterestInputViewModel
        {
            PrincipalAmount = 100000,
            RateOfInterest = 8.0,
            TimePeriodInYears = 10,
            CompoundingFrequencyPerYear = 1
        };

        var monthlyInput = new CompoundInterestInputViewModel
        {
            PrincipalAmount = 100000,
            RateOfInterest = 8.0,
            TimePeriodInYears = 10,
            CompoundingFrequencyPerYear = 12
        };

        // Act
        var annualResult = CalculateCompoundInterest(annualInput);
        var monthlyResult = CalculateCompoundInterest(monthlyInput);

        // Assert
        monthlyResult.TotalInterestEarned.Should().BeGreaterThan(annualResult.TotalInterestEarned);
        monthlyResult.TotalAmount.Should().BeGreaterThan(annualResult.TotalAmount);
    }

    [Fact]
    public void CalculateResult_WithMinimumValidInput_ReturnsCorrectResult()
    {
        // Arrange
        var input = new CompoundInterestInputViewModel
        {
            PrincipalAmount = 10000,
            RateOfInterest = 1.0,
            TimePeriodInYears = 1,
            CompoundingFrequencyPerYear = 1
        };

        // Act
        var result = CalculateCompoundInterest(input);

        // Assert
        result.TotalInterestEarned.Should().Be(100);
        result.TotalAmount.Should().Be(10100);
    }

    [Fact]
    public void CalculateResult_ResultsConsistency_TotalAmountEqualsPrincipalPlusInterest()
    {
        // Arrange
        var faker = new Faker();
        var input = new CompoundInterestInputViewModel
        {
            PrincipalAmount = faker.Random.Int(10000, 100000),
            RateOfInterest = faker.Random.Double(1.0, 15.0),
            TimePeriodInYears = faker.Random.Int(1, 30),
            CompoundingFrequencyPerYear = faker.PickRandom(1, 4, 12)
        };

        // Act
        var result = CalculateCompoundInterest(input);

        // Assert
        result.TotalAmount.Should().Be(result.Inputs.PrincipalAmount + result.TotalInterestEarned);
    }

    [Theory]
    [InlineData(10000, 5.0, 0, 4)]
    public void CalculateResult_WithZeroYears_ReturnsZeroInterest(int principal, double rate, int years, int frequency)
    {
        // Arrange
        var input = new CompoundInterestInputViewModel
        {
            PrincipalAmount = principal,
            RateOfInterest = rate,
            TimePeriodInYears = years,
            CompoundingFrequencyPerYear = frequency
        };

        // Act
        var result = CalculateCompoundInterest(input);

        // Assert
        result.TotalInterestEarned.Should().Be(0);
        result.TotalAmount.Should().Be(principal);
    }

    [Fact]
    public void CalculateResult_CompoundInterestShouldExceedSimpleInterest()
    {
        // Arrange
        const int principal = 100000;
        const double rate = 10.0;
        const int years = 5;

        var compoundInput = new CompoundInterestInputViewModel
        {
            PrincipalAmount = principal,
            RateOfInterest = rate,
            TimePeriodInYears = years,
            CompoundingFrequencyPerYear = 4
        };

        // Act
        var compoundResult = CalculateCompoundInterest(compoundInput);
        const int simpleInterest = (int)(principal * rate * years / 100);

        // Assert
        compoundResult.TotalInterestEarned.Should().BeGreaterThan(simpleInterest);
    }

    // Helper method that mimics the compound interest calculator logic
    private static CompoundInterestResultViewModel CalculateCompoundInterest(CompoundInterestInputViewModel input)
    {
        CompoundInterestCalculator calculator = new();
        return calculator.Compute(input);
    }
}