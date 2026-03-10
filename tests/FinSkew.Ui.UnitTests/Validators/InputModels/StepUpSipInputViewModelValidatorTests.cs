namespace FinSkew.Ui.UnitTests.Validators.InputModels;

public class StepUpSipInputViewModelValidatorTests
{
    #region Boundary Cases

    [Theory]
    [InlineData(500, 0, 1, 1)]
    [InlineData(10000000, 50, 100, 50)]
    public void Validate_WithBoundaryValues_ReturnsNoErrors(
        int monthlyInvestment,
        double stepUpPercentage,
        double expectedReturnRate,
        int timePeriodInYears)
    {
        // Arrange
        var input = new StepUpSipInputViewModel
        {
            MonthlyInvestment = monthlyInvestment,
            StepUpPercentage = stepUpPercentage,
            ExpectedReturnRate = expectedReturnRate,
            TimePeriodInYears = timePeriodInYears
        };
        var validator = new StepUpSipInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion

    #region Positive Cases

    [Fact]
    public void Validate_WithValidInput_ReturnsNoErrors()
    {
        // Arrange
        var input = new StepUpSipInputViewModel
        {
            MonthlyInvestment = 1000,
            StepUpPercentage = 5.0,
            ExpectedReturnRate = 12.0,
            TimePeriodInYears = 5
        };
        var validator = new StepUpSipInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WithZeroStepUpPercentage_ReturnsNoErrors()
    {
        // Arrange - 0 is the valid lower bound for StepUpPercentage
        var input = new StepUpSipInputViewModel
        {
            MonthlyInvestment = 1000,
            StepUpPercentage = 0,
            ExpectedReturnRate = 12.0,
            TimePeriodInYears = 5
        };
        var validator = new StepUpSipInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    #endregion

    #region Negative Cases

    [Theory]
    [InlineData(499, 5.0, 12.0, 5, nameof(StepUpSipInputViewModel.MonthlyInvestment), "Monthly investment must be between 500 and 10000000.")]
    [InlineData(10000001, 5.0, 12.0, 5, nameof(StepUpSipInputViewModel.MonthlyInvestment), "Monthly investment must be between 500 and 10000000.")]
    [InlineData(1000, -1, 12.0, 5, nameof(StepUpSipInputViewModel.StepUpPercentage), "Step-up percentage must be between 0 and 50.")]
    [InlineData(1000, 51, 12.0, 5, nameof(StepUpSipInputViewModel.StepUpPercentage), "Step-up percentage must be between 0 and 50.")]
    [InlineData(1000, 5.0, 0.5, 5, nameof(StepUpSipInputViewModel.ExpectedReturnRate), "Expected return rate must be between 1 and 100.")]
    [InlineData(1000, 5.0, 101, 5, nameof(StepUpSipInputViewModel.ExpectedReturnRate), "Expected return rate must be between 1 and 100.")]
    [InlineData(1000, 5.0, 12.0, 0, nameof(StepUpSipInputViewModel.TimePeriodInYears), "Time period in years must be between 1 and 50.")]
    [InlineData(1000, 5.0, 12.0, 51, nameof(StepUpSipInputViewModel.TimePeriodInYears), "Time period in years must be between 1 and 50.")]
    public void Validate_WithInvalidField_ReturnsExpectedErrorMessage(
        int monthlyInvestment,
        double stepUpPercentage,
        double expectedReturnRate,
        int timePeriodInYears,
        string expectedPropertyName,
        string expectedMessage)
    {
        // Arrange
        var input = new StepUpSipInputViewModel
        {
            MonthlyInvestment = monthlyInvestment,
            StepUpPercentage = stepUpPercentage,
            ExpectedReturnRate = expectedReturnRate,
            TimePeriodInYears = timePeriodInYears
        };
        var validator = new StepUpSipInputViewModelValidator();

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
        var input = new StepUpSipInputViewModel
        {
            MonthlyInvestment = 0,
            StepUpPercentage = -1,
            ExpectedReturnRate = 0,
            TimePeriodInYears = 0
        };
        var validator = new StepUpSipInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(StepUpSipInputViewModel.MonthlyInvestment) &&
            error.ErrorMessage == "Monthly investment must be between 500 and 10000000.");
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(StepUpSipInputViewModel.StepUpPercentage) &&
            error.ErrorMessage == "Step-up percentage must be between 0 and 50.");
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(StepUpSipInputViewModel.ExpectedReturnRate) &&
            error.ErrorMessage == "Expected return rate must be between 1 and 100.");
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(StepUpSipInputViewModel.TimePeriodInYears) &&
            error.ErrorMessage == "Time period in years must be between 1 and 50.");
    }

    #endregion
}