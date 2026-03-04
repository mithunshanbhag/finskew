namespace FinSkew.Ui.UnitTests.CalculationLogic;

public class RecurringDepositCalculationTests
{
    [Theory]
    [InlineData(5000, 6.0, 5, 300000, 348850, 48850)]
    [InlineData(1000, 12.0, 1, 12000, 12682, 682)]
    [InlineData(10000, 8.5, 10, 1200000, 1881384, 681384)]
    public void CalculateResult_WithValidInputs_ReturnsExpectedTotals(
        int monthlyDepositAmount,
        double expectedAnnualInterestRate,
        int timePeriodInYears,
        int expectedTotalInvested,
        int expectedMaturityAmount,
        int expectedTotalGain)
    {
        // Arrange
        var input = new RecurringDepositInputViewModel
        {
            MonthlyDepositAmount = monthlyDepositAmount,
            ExpectedAnnualInterestRate = expectedAnnualInterestRate,
            TimePeriodInYears = timePeriodInYears
        };

        // Act
        var result = CreateRecurringDepositCalculator().Compute(input);

        // Assert
        result.Should().NotBeNull();
        result.Inputs.Should().Be(input);
        result.TotalInvested.Should().Be(expectedTotalInvested);
        result.MaturityAmount.Should().Be(expectedMaturityAmount);
        result.TotalGain.Should().Be(expectedTotalGain);
    }

    [Theory]
    [InlineData(2500, 0.0, 7, 210000)]
    [InlineData(10000, 0.0, 2, 240000)]
    public void CalculateResult_WithZeroInterestRate_ReturnsInvestedAmountOnly(
        int monthlyDepositAmount,
        double expectedAnnualInterestRate,
        int timePeriodInYears,
        int expectedTotalInvested)
    {
        // Arrange
        var input = new RecurringDepositInputViewModel
        {
            MonthlyDepositAmount = monthlyDepositAmount,
            ExpectedAnnualInterestRate = expectedAnnualInterestRate,
            TimePeriodInYears = timePeriodInYears
        };

        // Act
        var result = CreateRecurringDepositCalculator().Compute(input);

        // Assert
        result.TotalInvested.Should().Be(expectedTotalInvested);
        result.MaturityAmount.Should().Be(expectedTotalInvested);
        result.TotalGain.Should().Be(0);
    }

    [Fact]
    public void CalculateResult_ResultConsistency_MaturityEqualsInvestedPlusGain()
    {
        // Arrange
        var faker = new Faker();
        var input = new RecurringDepositInputViewModel
        {
            MonthlyDepositAmount = faker.Random.Int(1000, 1000000),
            ExpectedAnnualInterestRate = faker.Random.Double(0.0, 100.0),
            TimePeriodInYears = faker.Random.Int(1, 50)
        };

        // Act
        var result = CreateRecurringDepositCalculator().Compute(input);

        // Assert
        result.TotalInvested.Should().Be(input.MonthlyDepositAmount * 12 * input.TimePeriodInYears);
        result.TotalGain.Should().Be(result.MaturityAmount - result.TotalInvested);
        result.MaturityAmount.Should().Be(result.TotalInvested + result.TotalGain);
    }

    private static RecurringDepositCalculator CreateRecurringDepositCalculator()
    {
        return new RecurringDepositCalculator(new AlwaysValidValidator<RecurringDepositInputViewModel>());
    }
}