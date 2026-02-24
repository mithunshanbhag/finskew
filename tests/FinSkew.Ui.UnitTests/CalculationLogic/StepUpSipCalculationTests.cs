namespace FinSkew.Ui.UnitTests.CalculationLogic;

public class StepUpSipCalculationTests
{
    [Theory]
    [InlineData(1000, 5.0, 12.0, 5, 66307, 90105, 23798)]
    [InlineData(2000, 10.0, 10.0, 10, 382498, 609170, 226672)]
    [InlineData(5000, 0.0, 15.0, 3, 180000, 228397, 48397)]
    [InlineData(1500, 8.0, 12.0, 7, 160610, 243287, 82677)]
    [InlineData(3000, 15.0, 8.0, 2, 77400, 83958, 6558)]
    public void CalculateResult_WithValidInputs_ReturnsExpectedTotals(
        int monthlyInvestment,
        double stepUpPercentage,
        double expectedReturnRate,
        int timePeriodInYears,
        int expectedTotalInvested,
        int expectedMaturityAmount,
        int expectedTotalGain)
    {
        // Arrange
        var input = new StepUpSipInputViewModel
        {
            MonthlyInvestment = monthlyInvestment,
            StepUpPercentage = stepUpPercentage,
            ExpectedReturnRate = expectedReturnRate,
            TimePeriodInYears = timePeriodInYears
        };

        // Act
        var result = CreateStepUpSipCalculator().Compute(input);

        // Assert
        result.Should().NotBeNull();
        result.Inputs.Should().Be(input);
        result.TotalInvested.Should().Be(expectedTotalInvested);
        result.MaturityAmount.Should().Be(expectedMaturityAmount);
        result.TotalGain.Should().Be(expectedTotalGain);
        result.YearlyGrowth.Should().HaveCount(timePeriodInYears);
        result.YearlyGrowth.Last().Should().Be(expectedMaturityAmount);

        for (var year = 1; year <= timePeriodInYears; year++)
        {
            var yearInput = new StepUpSipInputViewModel
            {
                MonthlyInvestment = monthlyInvestment,
                StepUpPercentage = stepUpPercentage,
                ExpectedReturnRate = expectedReturnRate,
                TimePeriodInYears = year
            };
            result.YearlyGrowth[year - 1].Should().Be(CreateStepUpSipCalculator().Compute(yearInput).MaturityAmount);
        }
    }

    [Fact]
    public void CalculateResult_WithZeroStepUp_MatchesRegularSip()
    {
        // Arrange
        const int monthlyInvestment = 1000;
        const double expectedReturnRate = 12.0;
        const int timePeriodInYears = 1;

        var stepUpInput = new StepUpSipInputViewModel
        {
            MonthlyInvestment = monthlyInvestment,
            StepUpPercentage = 0.0,
            ExpectedReturnRate = expectedReturnRate,
            TimePeriodInYears = timePeriodInYears
        };

        var sipInput = new SipInputViewModel
        {
            MonthlyInvestment = monthlyInvestment,
            ExpectedReturnRate = expectedReturnRate,
            TimePeriodInYears = timePeriodInYears
        };

        // Act
        var stepUpResult = CreateStepUpSipCalculator().Compute(stepUpInput);
        var sipResult = CreateSipCalculator().Compute(sipInput);

        // Assert
        stepUpResult.TotalInvested.Should().Be(sipResult.TotalInvested);
        stepUpResult.MaturityAmount.Should().Be(sipResult.MaturityAmount);
        stepUpResult.TotalGain.Should().Be(sipResult.TotalGain);
        stepUpResult.YearlyGrowth.Should().Equal(sipResult.MaturityAmount);
    }

    [Fact]
    public void CalculateResult_WithHighStepUp_IncreasesTotalInvestedYearOverYear()
    {
        // Arrange
        var input = new StepUpSipInputViewModel
        {
            MonthlyInvestment = 1000,
            StepUpPercentage = 20.0,
            ExpectedReturnRate = 12.0,
            TimePeriodInYears = 5
        };

        // Act
        var result = CreateStepUpSipCalculator().Compute(input);

        // Assert
        result.Should().NotBeNull();
        result.TotalInvested.Should().BeGreaterThan(input.MonthlyInvestment * 12 * input.TimePeriodInYears);
        result.MaturityAmount.Should().BeGreaterThan(result.TotalInvested);
        result.TotalGain.Should().Be(result.MaturityAmount - result.TotalInvested);
        result.YearlyGrowth.Should().HaveCount(input.TimePeriodInYears);
        result.YearlyGrowth.Should().BeInAscendingOrder();
        result.YearlyGrowth.Last().Should().Be(result.MaturityAmount);
    }

    [Fact]
    public void CalculateResult_MaturityAmountGreaterThanTotalInvested()
    {
        // Arrange
        var input = new StepUpSipInputViewModel
        {
            MonthlyInvestment = 2000,
            StepUpPercentage = 10.0,
            ExpectedReturnRate = 10.0,
            TimePeriodInYears = 10
        };

        // Act
        var result = CreateStepUpSipCalculator().Compute(input);

        // Assert
        result.MaturityAmount.Should().BeGreaterThan(result.TotalInvested);
        result.TotalGain.Should().BePositive();
        result.YearlyGrowth.Last().Should().Be(result.MaturityAmount);
    }

    [Fact]
    public void CalculateResult_TotalGainEqualsMaturityMinusInvested()
    {
        // Arrange
        var input = new StepUpSipInputViewModel
        {
            MonthlyInvestment = 1500,
            StepUpPercentage = 8.0,
            ExpectedReturnRate = 12.0,
            TimePeriodInYears = 7
        };

        // Act
        var result = CreateStepUpSipCalculator().Compute(input);

        // Assert
        result.TotalGain.Should().Be(result.MaturityAmount - result.TotalInvested);
        result.YearlyGrowth.Last().Should().Be(result.MaturityAmount);
    }

    [Fact]
    public void CalculateResult_WithZeroYears_ReturnsNoGrowthAndNoAmounts()
    {
        // Arrange
        var input = new StepUpSipInputViewModel
        {
            MonthlyInvestment = 1000,
            StepUpPercentage = 5.0,
            ExpectedReturnRate = 12.0,
            TimePeriodInYears = 0
        };

        // Act
        var result = CreateStepUpSipCalculator().Compute(input);

        // Assert
        result.TotalInvested.Should().Be(0);
        result.MaturityAmount.Should().Be(0);
        result.TotalGain.Should().Be(0);
        result.YearlyGrowth.Should().BeEmpty();
    }

    private static StepUpSipCalculator CreateStepUpSipCalculator()
    {
        return new StepUpSipCalculator(new AlwaysValidValidator<StepUpSipInputViewModel>());
    }

    private static SipCalculator CreateSipCalculator()
    {
        return new SipCalculator(new AlwaysValidValidator<SipInputViewModel>());
    }
}