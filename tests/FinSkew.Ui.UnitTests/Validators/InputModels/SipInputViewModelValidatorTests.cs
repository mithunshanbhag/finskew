namespace FinSkew.Ui.UnitTests.Validators.InputModels;

public class SipInputViewModelValidatorTests
{
    #region Positive Cases

    [Fact]
    public void Validate_WithValidInput_ReturnsNoErrors()
    {
        // Arrange
        var input = new SipInputViewModel
        {
            MonthlyInvestment = 1000,
            ExpectedReturnRate = 12.0,
            TimePeriodInYears = 5
        };
        var validator = new SipInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    #endregion

    #region Boundary Cases

    [Theory]
    [InlineData(500, 1, 1)]
    [InlineData(10000000, 100, 50)]
    public void Validate_WithBoundaryValues_ReturnsNoErrors(
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
        var validator = new SipInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion

    #region Negative Cases

    [Theory]
    [InlineData(499, 12.0, 5, nameof(SipInputViewModel.MonthlyInvestment), "Monthly investment must be between 500 and 10000000.")]
    [InlineData(10000001, 12.0, 5, nameof(SipInputViewModel.MonthlyInvestment), "Monthly investment must be between 500 and 10000000.")]
    [InlineData(1000, 0.5, 5, nameof(SipInputViewModel.ExpectedReturnRate), "Expected return rate must be between 1 and 100.")]
    [InlineData(1000, 101, 5, nameof(SipInputViewModel.ExpectedReturnRate), "Expected return rate must be between 1 and 100.")]
    [InlineData(1000, 12.0, 0, nameof(SipInputViewModel.TimePeriodInYears), "Time period in years must be between 1 and 50.")]
    [InlineData(1000, 12.0, 51, nameof(SipInputViewModel.TimePeriodInYears), "Time period in years must be between 1 and 50.")]
    public void Validate_WithInvalidField_ReturnsExpectedErrorMessage(
        int monthlyInvestment,
        double expectedReturnRate,
        int timePeriodInYears,
        string expectedPropertyName,
        string expectedMessage)
    {
        // Arrange
        var input = new SipInputViewModel
        {
            MonthlyInvestment = monthlyInvestment,
            ExpectedReturnRate = expectedReturnRate,
            TimePeriodInYears = timePeriodInYears
        };
        var validator = new SipInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == expectedPropertyName &&
            error.ErrorMessage == expectedMessage);
    }

    [Fact]
    public void Validate_WithAllInvalidFields_ReturnsAllErrorMessages()
    {
        // Arrange
        var input = new SipInputViewModel
        {
            MonthlyInvestment = 0,
            ExpectedReturnRate = 0,
            TimePeriodInYears = 0
        };
        var validator = new SipInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(SipInputViewModel.MonthlyInvestment) &&
            error.ErrorMessage == "Monthly investment must be between 500 and 10000000.");
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(SipInputViewModel.ExpectedReturnRate) &&
            error.ErrorMessage == "Expected return rate must be between 1 and 100.");
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(SipInputViewModel.TimePeriodInYears) &&
            error.ErrorMessage == "Time period in years must be between 1 and 50.");
    }

    #endregion
}