namespace FinSkew.Ui.UnitTests.CalculationLogic;

public class MisCalculationTests
{
    [Theory]
    [InlineData(100000, 6.6)]
    [InlineData(250000, 8.2)]
    public void CalculateResult_WithValidInputs_UsesQuarterlyCompoundingFormula(
        int investedAmount,
        double annualInterestRate)
    {
        // Arrange
        var input = new MisInputViewModel
        {
            InvestedAmount = investedAmount,
            AnnualInterestRate = annualInterestRate
        };
        var expectedByFormula = (int)(investedAmount *
            Math.Pow(1 + annualInterestRate / 100 / 4, 4 * input.TimePeriodInYears));

        // Act
        var result = CalculateMis(input);

        // Assert
        result.Should().NotBeNull();
        result.Inputs.Should().Be(input);
        result.FinalAmount.Should().Be(expectedByFormula);
        result.TotalGain.Should().Be(expectedByFormula - investedAmount);
    }

    [Fact]
    public void CalculateResult_ResultsConsistency_FinalAmountEqualsInvestedAmountPlusTotalGain()
    {
        // Arrange
        var faker = new Faker();
        var input = new MisInputViewModel
        {
            InvestedAmount = faker.Random.Int(10000, 9000000),
            AnnualInterestRate = faker.Random.Double(1.0, 15.0)
        };

        // Act
        var result = CalculateMis(input);

        // Assert
        result.FinalAmount.Should().Be(result.Inputs.InvestedAmount + result.TotalGain);
    }

    private static MisResultViewModel CalculateMis(MisInputViewModel input)
    {
        MisCalculator calculator = new(new AlwaysValidValidator<MisInputViewModel>());
        return calculator.Compute(input);
    }
}
