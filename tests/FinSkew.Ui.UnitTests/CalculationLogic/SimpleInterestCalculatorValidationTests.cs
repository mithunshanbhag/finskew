using ValidationException = FluentValidation.ValidationException;

namespace FinSkew.Ui.UnitTests.CalculationLogic;

public class SimpleInterestCalculatorValidationTests
{
    [Fact]
    public void Compute_WithValidInput_ReturnsExpectedResult()
    {
        // Arrange
        var input = new SimpleInterestInputViewModel
        {
            PrincipalAmount = 10000,
            RateOfInterest = 5,
            TimePeriodInYears = 3
        };
        var calculator = new SimpleInterestCalculator(new SimpleInterestInputViewModelValidator());

        // Act
        var result = calculator.Compute(input);

        // Assert
        result.TotalInterestEarned.Should().Be(1500);
        result.TotalAmount.Should().Be(11500);
    }

    [Theory]
    [InlineData(9999, 5, 3, "Principal amount must be between 10000 and 100000000.")]
    [InlineData(10000, 0.5, 3, "Rate of interest must be between 1 and 100.")]
    [InlineData(10000, 5, 0, "Time period in years must be between 1 and 100.")]
    public void Compute_WithInvalidInput_ThrowsValidationException(
        int principalAmount,
        double rateOfInterest,
        int timePeriodInYears,
        string expectedMessage)
    {
        // Arrange
        var input = new SimpleInterestInputViewModel
        {
            PrincipalAmount = principalAmount,
            RateOfInterest = rateOfInterest,
            TimePeriodInYears = timePeriodInYears
        };
        var calculator = new SimpleInterestCalculator(new SimpleInterestInputViewModelValidator());

        // Act
        var act = () => calculator.Compute(input);

        // Assert
        act.Should().Throw<ValidationException>()
            .Which.Errors.Select(error => error.ErrorMessage)
            .Should().Contain(expectedMessage);
    }
}