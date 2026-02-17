namespace FinSkew.Ui.UnitTests.CalculationLogic;

public class ScssCalculationTests
{
    [Theory]
    [InlineData(10000, 4289, 14289)]
    [InlineData(500000, 214482, 714482)]
    public void CalculateResult_WithValidInputs_ReturnsCorrectCalculation(
        int principal,
        int expectedInterest,
        int expectedMaturityAmount)
    {
        // Arrange
        var input = new ScssInputViewModel
        {
            PrincipalAmount = principal
        };

        // Act
        var result = CalculateScss(input);

        // Assert
        result.Should().NotBeNull();
        result.Inputs.Should().Be(input);
        result.TotalInterestEarned.Should().Be(expectedInterest);
        result.MaturityAmount.Should().Be(expectedMaturityAmount);
    }

    [Fact]
    public void CalculateResult_ResultsConsistency_MaturityAmountEqualsPrincipalPlusInterest()
    {
        // Arrange
        var faker = new Faker();
        var input = new ScssInputViewModel
        {
            PrincipalAmount = faker.Random.Int(10000, 10000000)
        };

        // Act
        var result = CalculateScss(input);

        // Assert
        result.MaturityAmount.Should().Be(result.Inputs.PrincipalAmount + result.TotalInterestEarned);
    }

    private static ScssResultViewModel CalculateScss(ScssInputViewModel input)
    {
        ScssCalculator calculator = new();
        return calculator.Compute(input);
    }
}