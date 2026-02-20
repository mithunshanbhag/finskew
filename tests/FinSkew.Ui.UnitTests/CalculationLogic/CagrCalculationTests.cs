namespace FinSkew.Ui.UnitTests.CalculationLogic;

public class CagrCalculationTests
{
    [Theory]
    [InlineData(10000, 12000, 3, 6.27)]
    [InlineData(100000, 150000, 5, 8.45)]
    [InlineData(50000, 100000, 10, 7.18)]
    [InlineData(25000, 50000, 7, 10.41)]
    [InlineData(10000, 26533, 10, 10.25)]
    [InlineData(100000, 161051, 5, 10.00)]
    public void Compute_WithValidInputs_ReturnsExpectedCAGR(
        int investedAmount,
        int finalAmount,
        int years,
        double expectedCagr)
    {
        // Arrange
        var input = new CagrInputViewModel
        {
            InitialPrincipalAmount = investedAmount,
            FinalAmount = finalAmount,
            TimePeriodInYears = years
        };

        // Act
        var result = CalculateCagr(input);

        // Assert
        result.Should().NotBeNull();
        result.Inputs.Should().Be(input);
        result.TotalGain.Should().Be(finalAmount - investedAmount);
        result.CompoundAnnualGrowthRate.Should().BeApproximately(expectedCagr, 0.01);
        result.YearlyGrowth.Should().HaveCount(years);
        result.YearlyGrowth[years - 1].Should().Be(finalAmount);
    }

    [Fact]
    public void Compute_WithMinimumValidInput_ReturnsCorrectResult()
    {
        // Arrange
        var input = new CagrInputViewModel
        {
            InitialPrincipalAmount = 10000,
            FinalAmount = 10100,
            TimePeriodInYears = 1
        };

        // Act
        var result = CalculateCagr(input);

        // Assert
        result.CompoundAnnualGrowthRate.Should().BeApproximately(1.00, 0.01);
        result.YearlyGrowth.Should().Equal([10100]);
    }

    [Fact]
    public void Compute_WithMaximumValidInput_ReturnsCorrectResult()
    {
        // Arrange
        var input = new CagrInputViewModel
        {
            InitialPrincipalAmount = 1000000,
            FinalAmount = 4045558,
            TimePeriodInYears = 10
        };

        // Act
        var result = CalculateCagr(input);

        // Assert
        result.CompoundAnnualGrowthRate.Should().BeApproximately(15.00, 0.01);
    }

    [Theory]
    [InlineData(100000, 80000, 5, -4.36)]
    [InlineData(50000, 40000, 3, -7.17)]
    [InlineData(25000, 20000, 10, -2.21)]
    [InlineData(100000, 50000, 7, -9.43)]
    public void Compute_WithFinalLessThanInvested_ReturnsNegativeCAGR(
        int investedAmount,
        int finalAmount,
        int years,
        double expectedCagr)
    {
        // Arrange
        var input = new CagrInputViewModel
        {
            InitialPrincipalAmount = investedAmount,
            FinalAmount = finalAmount,
            TimePeriodInYears = years
        };

        // Act
        var result = CalculateCagr(input);

        // Assert
        result.CompoundAnnualGrowthRate.Should().BeApproximately(expectedCagr, 0.01);
        result.CompoundAnnualGrowthRate.Should().BeNegative();
    }

    [Fact]
    public void Compute_WithNoGrowth_ReturnsZeroCAGR()
    {
        // Arrange
        var input = new CagrInputViewModel
        {
            InitialPrincipalAmount = 50000,
            FinalAmount = 50000,
            TimePeriodInYears = 5
        };

        // Act
        var result = CalculateCagr(input);

        // Assert
        result.CompoundAnnualGrowthRate.Should().BeApproximately(0.0, 0.01);
        result.TotalGain.Should().Be(0);
        result.YearlyGrowth.Should().HaveCount(input.TimePeriodInYears);
        result.YearlyGrowth.Should().OnlyContain(amount => amount == input.InitialPrincipalAmount);
    }

    [Theory]
    [InlineData(10000, 15000, 5)]
    [InlineData(50000, 75000, 7)]
    [InlineData(100000, 200000, 10)]
    [InlineData(25000, 40000, 8)]
    public void Compute_ResultConsistency_ReconstructedFinalAmountMatchesInput(
        int investedAmount,
        int finalAmount,
        int years)
    {
        // Arrange
        var input = new CagrInputViewModel
        {
            InitialPrincipalAmount = investedAmount,
            FinalAmount = finalAmount,
            TimePeriodInYears = years
        };

        // Act
        var result = CalculateCagr(input);

        // Assert - Reconstruct final amount using computed CAGR
        // FinalAmount = InvestedAmount × (1 + CAGR/100)^Years
        var reconstructedFinalAmount = investedAmount * Math.Pow(1 + result.CompoundAnnualGrowthRate / 100, years);
        reconstructedFinalAmount.Should().BeApproximately(finalAmount, 1.0);
    }

    [Fact]
    public void Compute_ResultConsistency_ReconstructedFinalAmountMatchesInputWithRandomValues()
    {
        // Arrange
        var faker = new Faker();
        var investedAmount = faker.Random.Int(10000, 100000);
        var finalAmount = faker.Random.Int(investedAmount + 1000, investedAmount * 3);
        var years = faker.Random.Int(1, 20);

        var input = new CagrInputViewModel
        {
            InitialPrincipalAmount = investedAmount,
            FinalAmount = finalAmount,
            TimePeriodInYears = years
        };

        // Act
        var result = CalculateCagr(input);

        // Assert - Reconstruct final amount using computed CAGR
        var reconstructedFinalAmount = investedAmount * Math.Pow(1 + result.CompoundAnnualGrowthRate / 100, years);
        reconstructedFinalAmount.Should().BeApproximately(finalAmount, 1.0);
    }

    [Theory]
    [InlineData(10000, 20000, 10, 7.18)]
    [InlineData(10000, 20000, 5, 14.87)]
    [InlineData(10000, 20000, 3, 25.99)]
    public void Compute_ShorterTimePeriod_ProducesHigherCAGR(
        int investedAmount,
        int finalAmount,
        int years,
        double expectedCagr)
    {
        // Arrange
        var input = new CagrInputViewModel
        {
            InitialPrincipalAmount = investedAmount,
            FinalAmount = finalAmount,
            TimePeriodInYears = years
        };

        // Act
        var result = CalculateCagr(input);

        // Assert
        result.CompoundAnnualGrowthRate.Should().BeApproximately(expectedCagr, 0.01);
    }

    [Fact]
    public void Compute_DoubleTheMoney_ProducesExpectedCAGRForDifferentPeriods()
    {
        // Arrange
        var shortTermInput = new CagrInputViewModel
        {
            InitialPrincipalAmount = 100000,
            FinalAmount = 200000,
            TimePeriodInYears = 5
        };

        var longTermInput = new CagrInputViewModel
        {
            InitialPrincipalAmount = 100000,
            FinalAmount = 200000,
            TimePeriodInYears = 10
        };

        // Act
        var shortTermResult = CalculateCagr(shortTermInput);
        var longTermResult = CalculateCagr(longTermInput);

        // Assert
        shortTermResult.CompoundAnnualGrowthRate.Should().BeGreaterThan(longTermResult.CompoundAnnualGrowthRate);
        shortTermResult.CompoundAnnualGrowthRate.Should().BeApproximately(14.87, 0.01);
        longTermResult.CompoundAnnualGrowthRate.Should().BeApproximately(7.18, 0.01);
    }

    [Fact]
    public void Compute_MultipleCallsWithSameInput_ReturnsSameResult()
    {
        // Arrange
        var input = new CagrInputViewModel
        {
            InitialPrincipalAmount = 75000,
            FinalAmount = 125000,
            TimePeriodInYears = 8
        };

        // Act
        var result1 = CalculateCagr(input);
        var result2 = CalculateCagr(input);

        // Assert
        result1.CompoundAnnualGrowthRate.Should().Be(result2.CompoundAnnualGrowthRate);
    }

    [Fact]
    public void Compute_VerifyFormulaAccuracy_ManualCalculation()
    {
        // Arrange - Using known values for manual verification
        // CAGR = ((FinalAmount / InvestedAmount)^(1/Years) - 1) × 100
        // CAGR = ((15000 / 10000)^(1/5) - 1) × 100
        // CAGR = (1.5^0.2 - 1) × 100 = (1.08447 - 1) × 100 = 8.447%
        var input = new CagrInputViewModel
        {
            InitialPrincipalAmount = 10000,
            FinalAmount = 15000,
            TimePeriodInYears = 5
        };

        // Act
        var result = CalculateCagr(input);

        // Assert
        result.CompoundAnnualGrowthRate.Should().BeApproximately(8.45, 0.01);
    }

    [Theory]
    [InlineData(10000, 17623, 5, 12.00)]
    [InlineData(50000, 94377, 7, 9.50)]
    [InlineData(100000, 259374, 10, 10.00)]
    public void Compute_WithHighGrowthRates_CalculatesAccurately(
        int investedAmount,
        int finalAmount,
        int years,
        double expectedCagr)
    {
        // Arrange
        var input = new CagrInputViewModel
        {
            InitialPrincipalAmount = investedAmount,
            FinalAmount = finalAmount,
            TimePeriodInYears = years
        };

        // Act
        var result = CalculateCagr(input);

        // Assert
        result.CompoundAnnualGrowthRate.Should().BeApproximately(expectedCagr, 0.01);
    }

    [Fact]
    public void Compute_WithDecimalPrecision_RoundsCorrectly()
    {
        // Arrange
        var input = new CagrInputViewModel
        {
            InitialPrincipalAmount = 12345,
            FinalAmount = 67890,
            TimePeriodInYears = 11
        };

        // Act
        var result = CalculateCagr(input);

        // Assert
        result.CompoundAnnualGrowthRate.Should().BeGreaterThan(0);
        result.CompoundAnnualGrowthRateStr.Should().Contain("%");
        result.InitialPrincipalAmountStr.Should().Contain("12,345");
        result.TotalGainStr.Should().Contain("55,545");
        result.FinalAmountStr.Should().Contain("67,890");
        // Verify that the string representation includes exactly 2 decimal places
        result.CompoundAnnualGrowthRateStr.Should().MatchRegex(@"\d+\.\d{2}%");
    }

    [Fact]
    public void Compute_WithLargeAmounts_HandlesCalculationCorrectly()
    {
        // Arrange
        var input = new CagrInputViewModel
        {
            InitialPrincipalAmount = 10000000,
            FinalAmount = 40455577,
            TimePeriodInYears = 10
        };

        // Act
        var result = CalculateCagr(input);

        // Assert
        result.CompoundAnnualGrowthRate.Should().BeApproximately(15.00, 0.01);
    }

    [Theory]
    [InlineData(1000, 2000, 1, 100.00)]
    [InlineData(1000, 3000, 1, 200.00)]
    [InlineData(1000, 4000, 1, 300.00)]
    public void Compute_SingleYearGrowth_CalculatesPercentageGainCorrectly(
        int investedAmount,
        int finalAmount,
        int years,
        double expectedCagr)
    {
        // Arrange
        var input = new CagrInputViewModel
        {
            InitialPrincipalAmount = investedAmount,
            FinalAmount = finalAmount,
            TimePeriodInYears = years
        };

        // Act
        var result = CalculateCagr(input);

        // Assert
        result.CompoundAnnualGrowthRate.Should().BeApproximately(expectedCagr, 0.01);
    }

    // Helper method that mimics the calculator logic
    private static CagrResultViewModel CalculateCagr(CagrInputViewModel input)
    {
        CagrCalculator calculator = new();
        return calculator.Compute(input);
    }
}
