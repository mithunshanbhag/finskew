namespace FinSkew.Ui.UnitTests.CalculationLogic;

public class LumpsumCalculationTests
{
    [Theory]
    [InlineData(10000, 5.0, 3, 1576, 11576)]
    [InlineData(10000, 1.0, 1, 100, 10100)]
    [InlineData(100000, 8.0, 5, 46932, 146932)]
    [InlineData(50000, 10.0, 2, 10500, 60500)]
    [InlineData(25000, 7.5, 4, 8386, 33386)]
    [InlineData(75000, 6.0, 10, 59313, 134313)]
    public void CalculateResult_WithValidInputs_ReturnsExpectedTotals(
        int principal,
        double rate,
        int years,
        int expectedGain,
        int expectedMaturityAmount)
    {
        // Arrange
        var input = new LumpsumInputViewModel
        {
            PrincipalAmount = principal,
            RateOfInterest = rate,
            TimePeriodInYears = years
        };

        // Act
        var result = CalculateLumpsum(input);

        // Assert
        result.Should().NotBeNull();
        result.Inputs.Should().Be(input);
        result.TotalGain.Should().Be(expectedGain);
        result.MaturityAmount.Should().Be(expectedMaturityAmount);
        result.YearlyGrowth.Should().HaveCount(years);
        for (var year = 1; year <= years; year++)
        {
            var expectedYearEndAmount = (int)(principal * Math.Pow(1 + rate / 100, year));
            result.YearlyGrowth[year - 1].Should().Be(expectedYearEndAmount);
        }
    }

    [Fact]
    public void CalculateResult_WithMinimumValidInput_ReturnsCorrectResult()
    {
        // Arrange
        var input = new LumpsumInputViewModel
        {
            PrincipalAmount = 10000,
            RateOfInterest = 1.0,
            TimePeriodInYears = 1
        };

        // Act
        var result = CalculateLumpsum(input);

        // Assert
        result.TotalGain.Should().Be(100);
        result.MaturityAmount.Should().Be(10100);
        result.YearlyGrowth.Should().Equal(10100);
    }

    [Fact]
    public void CalculateResult_WithMaximumValidInput_ReturnsCorrectResult()
    {
        // Arrange
        var input = new LumpsumInputViewModel
        {
            PrincipalAmount = 100000000,
            RateOfInterest = 15.0,
            TimePeriodInYears = 10
        };

        // Act
        var result = CalculateLumpsum(input);

        // Assert
        result.TotalGain.Should().Be(304555773);
        result.MaturityAmount.Should().Be(404555773);
        result.YearlyGrowth.Should().HaveCount(10);
        result.YearlyGrowth.Last().Should().Be(result.MaturityAmount);
    }

    [Theory]
    [InlineData(15000, 4.5, 7)]
    [InlineData(80000, 9.25, 15)]
    [InlineData(120000, 5.75, 20)]
    public void CalculateResult_WithDecimalRates_RoundsCorrectly(int principal, double rate, int years)
    {
        // Arrange
        var input = new LumpsumInputViewModel
        {
            PrincipalAmount = principal,
            RateOfInterest = rate,
            TimePeriodInYears = years
        };

        // Act
        var result = CalculateLumpsum(input);

        // Assert
        result.TotalGain.Should().BeGreaterThan(0);
        result.MaturityAmount.Should().Be(result.Inputs.PrincipalAmount + result.TotalGain);
        result.YearlyGrowth.Should().HaveCount(years);
        result.YearlyGrowth.Should().BeInAscendingOrder();
    }

    [Fact]
    public void CalculateResult_ResultsConsistency_MaturityAmountEqualsPrincipalPlusGain()
    {
        // Arrange
        var faker = new Faker();
        var input = new LumpsumInputViewModel
        {
            PrincipalAmount = faker.Random.Int(10000, 100000),
            RateOfInterest = faker.Random.Double(1.0, 15.0),
            TimePeriodInYears = faker.Random.Int(1, 30)
        };

        // Act
        var result = CalculateLumpsum(input);

        // Assert
        result.MaturityAmount.Should().Be(result.Inputs.PrincipalAmount + result.TotalGain);
        result.YearlyGrowth.Should().HaveCount(input.TimePeriodInYears);
        result.YearlyGrowth.Last().Should().Be(result.MaturityAmount);
    }

    [Theory]
    [InlineData(10000, 5.0, 0)]
    public void CalculateResult_WithZeroYears_ReturnsZeroGain(int principal, double rate, int years)
    {
        // Arrange
        var input = new LumpsumInputViewModel
        {
            PrincipalAmount = principal,
            RateOfInterest = rate,
            TimePeriodInYears = years
        };

        // Act
        var result = CalculateLumpsum(input);

        // Assert
        result.TotalGain.Should().Be(0);
        result.MaturityAmount.Should().Be(principal);
        result.YearlyGrowth.Should().BeEmpty();
    }

    [Theory]
    [InlineData(100000, 10.0, 1, 10000)]
    [InlineData(100000, 10.0, 2, 21000)]
    [InlineData(100000, 10.0, 3, 33100)]
    [InlineData(100000, 10.0, 5, 61051)]
    [InlineData(100000, 10.0, 10, 159374)]
    public void CalculateResult_WithIncreasingYears_GainGrowsExponentially(
        int principal,
        double rate,
        int years,
        int expectedGain)
    {
        // Arrange
        var input = new LumpsumInputViewModel
        {
            PrincipalAmount = principal,
            RateOfInterest = rate,
            TimePeriodInYears = years
        };

        // Act
        var result = CalculateLumpsum(input);

        // Assert
        result.TotalGain.Should().Be(expectedGain);
    }

    [Fact]
    public void CalculateResult_LongerTimePeriod_ProducesMoreGainThanShorterPeriod()
    {
        // Arrange
        var shortTermInput = new LumpsumInputViewModel
        {
            PrincipalAmount = 100000,
            RateOfInterest = 8.0,
            TimePeriodInYears = 5
        };

        var longTermInput = new LumpsumInputViewModel
        {
            PrincipalAmount = 100000,
            RateOfInterest = 8.0,
            TimePeriodInYears = 10
        };

        // Act
        var shortTermResult = CalculateLumpsum(shortTermInput);
        var longTermResult = CalculateLumpsum(longTermInput);

        // Assert
        longTermResult.TotalGain.Should().BeGreaterThan(shortTermResult.TotalGain);
        longTermResult.MaturityAmount.Should().BeGreaterThan(shortTermResult.MaturityAmount);
    }

    [Fact]
    public void CalculateResult_HigherRate_ProducesMoreGainThanLowerRate()
    {
        // Arrange
        var lowRateInput = new LumpsumInputViewModel
        {
            PrincipalAmount = 100000,
            RateOfInterest = 5.0,
            TimePeriodInYears = 10
        };

        var highRateInput = new LumpsumInputViewModel
        {
            PrincipalAmount = 100000,
            RateOfInterest = 10.0,
            TimePeriodInYears = 10
        };

        // Act
        var lowRateResult = CalculateLumpsum(lowRateInput);
        var highRateResult = CalculateLumpsum(highRateInput);

        // Assert
        highRateResult.TotalGain.Should().BeGreaterThan(lowRateResult.TotalGain);
        highRateResult.MaturityAmount.Should().BeGreaterThan(lowRateResult.MaturityAmount);
    }

    [Theory]
    [InlineData(10000, 12.0, 5, 7623)]
    [InlineData(50000, 9.5, 7, 44377)]
    [InlineData(200000, 6.25, 15, 296551)]
    public void CalculateResult_WithHighRatesAndLongTerms_CalculatesAccurately(
        int principal,
        double rate,
        int years,
        int expectedGain)
    {
        // Arrange
        var input = new LumpsumInputViewModel
        {
            PrincipalAmount = principal,
            RateOfInterest = rate,
            TimePeriodInYears = years
        };

        // Act
        var result = CalculateLumpsum(input);

        // Assert
        result.TotalGain.Should().Be(expectedGain);
        result.MaturityAmount.Should().Be(principal + expectedGain);
    }

    [Fact]
    public void CalculateResult_MultipleCallsWithSameInput_ReturnsSameResult()
    {
        // Arrange
        var input = new LumpsumInputViewModel
        {
            PrincipalAmount = 75000,
            RateOfInterest = 7.5,
            TimePeriodInYears = 8
        };

        // Act
        var result1 = CalculateLumpsum(input);
        var result2 = CalculateLumpsum(input);

        // Assert
        result1.TotalGain.Should().Be(result2.TotalGain);
        result1.MaturityAmount.Should().Be(result2.MaturityAmount);
    }

    [Fact]
    public void CalculateResult_VerifyFormulaAccuracy_ManualCalculation()
    {
        // Arrange - Using known values for manual verification
        // A = P × (1 + R/100)^N
        // A = 10000 × (1 + 5/100)^3 = 10000 × 1.05^3 = 10000 × 1.157625 = 11576.25 ≈ 11576
        // I = A - P = 11576 - 10000 = 1576
        var input = new LumpsumInputViewModel
        {
            PrincipalAmount = 10000,
            RateOfInterest = 5.0,
            TimePeriodInYears = 3
        };

        // Act
        var result = CalculateLumpsum(input);

        // Assert
        result.TotalGain.Should().Be(1576);
        result.MaturityAmount.Should().Be(11576);
        result.YearlyGrowth.Should().Equal(10500, 11025, 11576);
    }

    // Helper method that mimics the calculator logic
    private static LumpsumResultViewModel CalculateLumpsum(LumpsumInputViewModel input)
    {
        LumpsumCalculator calculator = new();
        return calculator.Compute(input);
    }
}