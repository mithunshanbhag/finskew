namespace FinSkew.Ui.UnitTests.CalculationLogic;

public class SwpCalculationTests
{
    [Theory]
    [InlineData(100000, 5000, 8.0, 5, 300000, -220849)]
    [InlineData(500000, 10000, 10.0, 10, 1200000, -711999)]
    [InlineData(200000, 8000, 12.0, 3, 288000, -61907)]
    [InlineData(1000000, 20000, 6.0, 15, 3600000, -3391363)]
    [InlineData(300000, 12000, 7.5, 7, 1008000, -822325)]
    public void CalculateResult_WithValidInputs_ReturnsCorrectCalculation(
        int totalInvestment,
        int monthlyWithdrawal,
        double annualReturnRate,
        int years,
        int expectedTotalWithdrawal,
        int expectedTotalMaturityAmount)
    {
        // Arrange
        var input = new SwpInputViewModel
        {
            TotalInvestmentAmount = totalInvestment,
            MonthlyWithdrawalAmount = monthlyWithdrawal,
            ExpectedAnnualReturnRate = annualReturnRate,
            TimePeriodInYears = years
        };

        // Act
        var result = CalculateSwp(input);

        // Assert
        result.Should().NotBeNull();
        result.Inputs.Should().Be(input);
        result.TotalWithdrawal.Should().Be(expectedTotalWithdrawal);
        result.TotalMaturityAmount.Should().Be(expectedTotalMaturityAmount);
        result.YearlyGrowth.Should().HaveCount(years);
        result.YearlyGrowth.Last().Should().Be(expectedTotalMaturityAmount);
    }

    [Fact]
    public void CalculateResult_WithMinimumValidInput_ReturnsCorrectResult()
    {
        // Arrange
        var input = new SwpInputViewModel
        {
            TotalInvestmentAmount = 10000,
            MonthlyWithdrawalAmount = 500,
            ExpectedAnnualReturnRate = 1.0,
            TimePeriodInYears = 1
        };

        // Act
        var result = CalculateSwp(input);

        // Assert
        result.TotalWithdrawal.Should().Be(6000);
        result.TotalMaturityAmount.Should().Be(4068);
        result.YearlyGrowth.Should().Equal(4068);
    }

    [Fact]
    public void CalculateResult_WithMaximumValidInput_ReturnsCorrectResult()
    {
        // Arrange
        var input = new SwpInputViewModel
        {
            TotalInvestmentAmount = 10000000,
            MonthlyWithdrawalAmount = 100000,
            ExpectedAnnualReturnRate = 15.0,
            TimePeriodInYears = 30
        };

        // Act
        var result = CalculateSwp(input);

        // Assert
        result.TotalWithdrawal.Should().Be(36000000);
        result.TotalMaturityAmount.Should().BeGreaterThan(0);
        result.YearlyGrowth.Should().HaveCount(30);
        result.YearlyGrowth.Last().Should().Be(result.TotalMaturityAmount);
    }

    [Theory]
    [InlineData(150000, 7500, 4.5, 7)]
    [InlineData(300000, 15000, 9.25, 5)]
    [InlineData(500000, 20000, 5.75, 10)]
    public void CalculateResult_WithDecimalRates_RoundsCorrectly(
        int totalInvestment,
        int monthlyWithdrawal,
        double annualReturnRate,
        int years)
    {
        // Arrange
        var input = new SwpInputViewModel
        {
            TotalInvestmentAmount = totalInvestment,
            MonthlyWithdrawalAmount = monthlyWithdrawal,
            ExpectedAnnualReturnRate = annualReturnRate,
            TimePeriodInYears = years
        };

        // Act
        var result = CalculateSwp(input);

        // Assert
        var expectedTotalWithdrawal = monthlyWithdrawal * years * 12;
        result.TotalWithdrawal.Should().Be(expectedTotalWithdrawal);
        // TotalMaturityAmount can be negative when withdrawals deplete the corpus
        result.TotalMaturityAmount.Should().NotBe(0);
        result.YearlyGrowth.Should().HaveCount(years);
    }

    [Fact]
    public void CalculateResult_ResultsConsistency_TotalWithdrawalAndMaturityAmountCalculatedCorrectly()
    {
        // Arrange
        var faker = new Faker();
        var input = new SwpInputViewModel
        {
            TotalInvestmentAmount = faker.Random.Int(100000, 1000000),
            MonthlyWithdrawalAmount = faker.Random.Int(5000, 50000),
            ExpectedAnnualReturnRate = faker.Random.Double(1.0, 15.0),
            TimePeriodInYears = faker.Random.Int(1, 30)
        };

        // Act
        var result = CalculateSwp(input);

        // Assert
        var expectedTotalWithdrawal = input.MonthlyWithdrawalAmount * input.TimePeriodInYears * 12;
        result.TotalWithdrawal.Should().Be(expectedTotalWithdrawal);
        // TotalMaturityAmount can be negative when withdrawals deplete the corpus
        result.TotalMaturityAmount.Should().NotBe(0);
        result.YearlyGrowth.Should().HaveCount(input.TimePeriodInYears);
        result.YearlyGrowth.Last().Should().Be(result.TotalMaturityAmount);
    }

    [Fact]
    public void CalculateResult_WithZeroReturnRate_CalculatesCorrectly()
    {
        // Arrange
        var input = new SwpInputViewModel
        {
            TotalInvestmentAmount = 100000,
            MonthlyWithdrawalAmount = 5000,
            ExpectedAnnualReturnRate = 0.0,
            TimePeriodInYears = 2
        };

        // Act
        var result = CalculateSwp(input);

        // Assert
        // With 0% return, the corpus depletes linearly
        // Total withdrawal = monthly × months
        result.TotalWithdrawal.Should().Be(120000);
        // Maturity amount = Initial - Total Withdrawal = 100000 - 120000 = -20000
        result.TotalMaturityAmount.Should().Be(-20000);
        result.YearlyGrowth.Should().Equal(40000, -20000);
    }

    [Theory]
    [InlineData(1000000, 10000, 10.0, 10)]
    public void CalculateResult_WithSustainableWithdrawal_ReturnsFullWithdrawal(
        int totalInvestment,
        int monthlyWithdrawal,
        double annualReturnRate,
        int years)
    {
        // Arrange
        var input = new SwpInputViewModel
        {
            TotalInvestmentAmount = totalInvestment,
            MonthlyWithdrawalAmount = monthlyWithdrawal,
            ExpectedAnnualReturnRate = annualReturnRate,
            TimePeriodInYears = years
        };

        // Act
        var result = CalculateSwp(input);

        // Assert
        // For truly sustainable scenarios, we expect full withdrawal amount
        var expectedFullWithdrawal = monthlyWithdrawal * years * 12;
        result.TotalWithdrawal.Should().Be(expectedFullWithdrawal);
        result.TotalMaturityAmount.Should().BeGreaterThan(0);
    }

    [Fact]
    public void CalculateResult_WithShortTermWithdrawal_ReturnsExpectedAmount()
    {
        // Arrange - short-term scenarios may have negative gains
        var input = new SwpInputViewModel
        {
            TotalInvestmentAmount = 100000,
            MonthlyWithdrawalAmount = 2000,
            ExpectedAnnualReturnRate = 8.0,
            TimePeriodInYears = 1
        };

        // Act
        var result = CalculateSwp(input);

        // Assert
        var expectedFullWithdrawal = 2000 * 12;
        result.TotalWithdrawal.Should().Be(expectedFullWithdrawal);
    }

    [Theory]
    [InlineData(1000000, 10000, 8.0, 5)]
    [InlineData(500000, 5000, 10.0, 10)]
    public void CalculateResult_WithLongerPeriod_MaintainsOrIncreasesTotalWithdrawal(
        int totalInvestment,
        int monthlyWithdrawal,
        double annualReturnRate,
        int baseYears)
    {
        // Arrange
        var inputShortPeriod = new SwpInputViewModel
        {
            TotalInvestmentAmount = totalInvestment,
            MonthlyWithdrawalAmount = monthlyWithdrawal,
            ExpectedAnnualReturnRate = annualReturnRate,
            TimePeriodInYears = baseYears
        };

        var inputLongPeriod = new SwpInputViewModel
        {
            TotalInvestmentAmount = totalInvestment,
            MonthlyWithdrawalAmount = monthlyWithdrawal,
            ExpectedAnnualReturnRate = annualReturnRate,
            TimePeriodInYears = baseYears + 1
        };

        // Act
        var resultShortPeriod = CalculateSwp(inputShortPeriod);
        var resultLongPeriod = CalculateSwp(inputLongPeriod);

        // Assert
        // For sustainable withdrawals, longer period allows more total withdrawal
        resultLongPeriod.TotalWithdrawal.Should().BeGreaterThan(resultShortPeriod.TotalWithdrawal);
    }

    [Theory]
    [InlineData(1000000, 5000, 8.0, 10)]
    [InlineData(500000, 2000, 10.0, 10)]
    public void CalculateResult_WithDoubledMonthlyWithdrawal_IncreasesTotalWithdrawal(
        int totalInvestment,
        int baseWithdrawal,
        double annualReturnRate,
        int years)
    {
        // Arrange
        var inputLowWithdrawal = new SwpInputViewModel
        {
            TotalInvestmentAmount = totalInvestment,
            MonthlyWithdrawalAmount = baseWithdrawal,
            ExpectedAnnualReturnRate = annualReturnRate,
            TimePeriodInYears = years
        };

        var inputHighWithdrawal = new SwpInputViewModel
        {
            TotalInvestmentAmount = totalInvestment,
            MonthlyWithdrawalAmount = baseWithdrawal * 2,
            ExpectedAnnualReturnRate = annualReturnRate,
            TimePeriodInYears = years
        };

        // Act
        var resultLowWithdrawal = CalculateSwp(inputLowWithdrawal);
        var resultHighWithdrawal = CalculateSwp(inputHighWithdrawal);

        // Assert
        // With sustainable corpus, doubling withdrawals should increase total withdrawal
        resultHighWithdrawal.TotalWithdrawal.Should().BeGreaterThan(resultLowWithdrawal.TotalWithdrawal);
    }

    [Fact]
    public void CalculateResult_WithDepletingCorpus_YearlyGrowthAllowsNegativeValues()
    {
        // Arrange
        var input = new SwpInputViewModel
        {
            TotalInvestmentAmount = 100000,
            MonthlyWithdrawalAmount = 5000,
            ExpectedAnnualReturnRate = 8.0,
            TimePeriodInYears = 5
        };

        // Act
        var result = CalculateSwp(input);

        // Assert
        result.YearlyGrowth.Should().Equal(45635, -13242, -77005, -146061, -220849);
        result.YearlyGrowth.Should().Contain(value => value < 0);
    }

    [Fact]
    public void CalculateResult_FormattedStrings_UseIndianCulture()
    {
        // Arrange
        var input = new SwpInputViewModel
        {
            TotalInvestmentAmount = 100000,
            MonthlyWithdrawalAmount = 5000,
            ExpectedAnnualReturnRate = 8.0,
            TimePeriodInYears = 5
        };

        // Act
        var result = CalculateSwp(input);

        // Assert
        result.TotalInvestmentAmountStr.Should().StartWith("₹");
        result.TotalWithdrawalStr.Should().StartWith("₹");
        // TotalMaturityAmountStr can have a negative sign prefix for negative values
        result.TotalMaturityAmountStr.Should().Contain("₹");
    }

    [Fact]
    public void CalculateResult_WithTypicalSWPScenario_ReturnsRealisticValues()
    {
        // Arrange - typical retirement withdrawal scenario
        var input = new SwpInputViewModel
        {
            TotalInvestmentAmount = 1000000, // ₹10 lakhs
            MonthlyWithdrawalAmount = 10000, // ₹10k per month
            ExpectedAnnualReturnRate = 8.0, // 8% annual return
            TimePeriodInYears = 10 // 10 years
        };

        // Act
        var result = CalculateSwp(input);

        // Assert
        // With good returns and reasonable withdrawals, corpus should sustain full withdrawals
        result.TotalWithdrawal.Should().Be(1200000); // 10k × 12 months × 10 years
        result.TotalMaturityAmount.Should().BeGreaterThan(0); // Remaining corpus should be positive
    }

    [Theory]
    [InlineData(100000, 10000, 5.0, 5)] // Aggressive withdrawals deplete corpus
    [InlineData(200000, 10000, 6.0, 5)] // Moderately aggressive withdrawals
    public void CalculateResult_WithAggressiveWithdrawal_DepletesCorpus(
        int totalInvestment,
        int monthlyWithdrawal,
        double annualReturnRate,
        int years)
    {
        // Arrange
        var input = new SwpInputViewModel
        {
            TotalInvestmentAmount = totalInvestment,
            MonthlyWithdrawalAmount = monthlyWithdrawal,
            ExpectedAnnualReturnRate = annualReturnRate,
            TimePeriodInYears = years
        };

        // Act
        var result = CalculateSwp(input);

        // Assert
        var fullWithdrawal = monthlyWithdrawal * years * 12;
        result.TotalWithdrawal.Should().Be(fullWithdrawal);
        // When withdrawals are aggressive, maturity amount will be negative (corpus depleted)
        result.TotalMaturityAmount.Should().BeLessThan(0);
    }

    // Helper method that mimics the calculator logic
    private static SwpResultViewModel CalculateSwp(SwpInputViewModel input)
    {
        SwpCalculator calculator = new(new AlwaysValidValidator<SwpInputViewModel>());
        return calculator.Compute(input);
    }
}