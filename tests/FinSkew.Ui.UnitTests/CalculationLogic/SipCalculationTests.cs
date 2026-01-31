namespace FinSkew.Ui.UnitTests.CalculationLogic;

public class SipCalculationTests
{
    [Theory]
    [InlineData(1000, 12.0, 1, 12000, 12809, 809)]
    [InlineData(500, 10.0, 5, 30000, 39041, 9041)]
    [InlineData(2000, 15.0, 10, 240000, 557314, 317314)]
    [InlineData(1500, 8.0, 3, 54000, 61208, 7208)]
    [InlineData(2500, 20.0, 7, 210000, 458823, 248823)]
    public void CalculateResult_WithValidInputs_ReturnsExpectedTotals(
        int monthlyInvestment,
        double expectedReturnRate,
        int timePeriodInYears,
        int expectedTotalInvested,
        int expectedMaturityAmount,
        int expectedTotalGain)
    {
        // Arrange
        var input = new SipInputViewModel
        {
            MonthlyInvestment = monthlyInvestment,
            ExpectedReturnRate = expectedReturnRate,
            TimePeriodInYears = timePeriodInYears
        };

        // Act
        var result = new SipCalculator().Compute(input);

        // Assert
        result.Should().NotBeNull();
        result.Inputs.Should().Be(input);
        result.TotalInvested.Should().Be(expectedTotalInvested);
        result.MaturityAmount.Should().Be(expectedMaturityAmount);
        result.TotalGain.Should().Be(expectedTotalGain);
    }

    [Theory]
    [InlineData(5000, 12.0, 20, 1200000, 4995739)]
    [InlineData(10000, 15.0, 15, 1800000, 6768630)]
    [InlineData(3000, 10.0, 25, 900000, 4013671)]
    public void CalculateResult_WithLargeTimePeriods_ReturnsExpectedTotals(
        int monthlyInvestment,
        double expectedReturnRate,
        int timePeriodInYears,
        int expectedTotalInvested,
        int expectedMaturityAmount)
    {
        // Arrange
        var input = new SipInputViewModel
        {
            MonthlyInvestment = monthlyInvestment,
            ExpectedReturnRate = expectedReturnRate,
            TimePeriodInYears = timePeriodInYears
        };

        // Act
        var result = new SipCalculator().Compute(input);

        // Assert
        result.Should().NotBeNull();
        result.TotalInvested.Should().Be(expectedTotalInvested);
        result.MaturityAmount.Should().Be(expectedMaturityAmount);
        result.TotalGain.Should().Be(expectedMaturityAmount - expectedTotalInvested);
    }

    [Theory]
    [InlineData(1000, 0.0, 5, 60000, 60000, 0)]
    [InlineData(2000, 0.0, 10, 240000, 240000, 0)]
    public void CalculateResult_WithZeroReturnRate_ReturnsInvestedAmountOnly(
        int monthlyInvestment,
        double expectedReturnRate,
        int timePeriodInYears,
        int expectedTotalInvested,
        int expectedMaturityAmount,
        int expectedTotalGain)
    {
        // Arrange
        var input = new SipInputViewModel
        {
            MonthlyInvestment = monthlyInvestment,
            ExpectedReturnRate = expectedReturnRate,
            TimePeriodInYears = timePeriodInYears
        };

        // Act
        var result = new SipCalculator().Compute(input);

        // Assert
        result.Should().NotBeNull();
        result.TotalInvested.Should().Be(expectedTotalInvested);
        result.MaturityAmount.Should().Be(expectedMaturityAmount);
        result.TotalGain.Should().Be(expectedTotalGain);
    }

    [Theory]
    [InlineData(1000, 12.0, 1)]
    [InlineData(5000, 15.0, 5)]
    [InlineData(10000, 10.0, 10)]
    public void CalculateResult_TotalInvestedShouldEqualMonthlyTimesMonths(
        int monthlyInvestment,
        double expectedReturnRate,
        int timePeriodInYears)
    {
        // Arrange
        var input = new SipInputViewModel
        {
            MonthlyInvestment = monthlyInvestment,
            ExpectedReturnRate = expectedReturnRate,
            TimePeriodInYears = timePeriodInYears
        };

        // Act
        var result = new SipCalculator().Compute(input);

        // Assert
        result.TotalInvested.Should().Be(monthlyInvestment * 12 * timePeriodInYears);
    }

    [Theory]
    [InlineData(1000, 12.0, 1)]
    [InlineData(500, 10.0, 5)]
    [InlineData(2000, 15.0, 10)]
    public void CalculateResult_TotalGainShouldEqualMaturityMinusInvested(
        int monthlyInvestment,
        double expectedReturnRate,
        int timePeriodInYears)
    {
        // Arrange
        var input = new SipInputViewModel
        {
            MonthlyInvestment = monthlyInvestment,
            ExpectedReturnRate = expectedReturnRate,
            TimePeriodInYears = timePeriodInYears
        };

        // Act
        var result = new SipCalculator().Compute(input);

        // Assert
        result.TotalGain.Should().Be(result.MaturityAmount - result.TotalInvested);
    }

    [Theory]
    [InlineData(100, 8.0, 1, 1200, 1253)]
    [InlineData(25000, 18.0, 15, 4500000, 22980222)]
    public void CalculateResult_WithEdgeCaseValues_ReturnsExpectedTotals(
        int monthlyInvestment,
        double expectedReturnRate,
        int timePeriodInYears,
        int expectedTotalInvested,
        int expectedMaturityAmount)
    {
        // Arrange
        var input = new SipInputViewModel
        {
            MonthlyInvestment = monthlyInvestment,
            ExpectedReturnRate = expectedReturnRate,
            TimePeriodInYears = timePeriodInYears
        };

        // Act
        var result = new SipCalculator().Compute(input);

        // Assert
        result.Should().NotBeNull();
        result.TotalInvested.Should().Be(expectedTotalInvested);
        result.MaturityAmount.Should().Be(expectedMaturityAmount);
    }
}