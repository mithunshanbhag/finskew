namespace FinSkew.Ui.UnitTests.Validators.InputModels;

public class RecurringDepositInputViewModelValidatorTests
{
    #region Boundary Cases

    [Theory]
    [InlineData(1000, 0, 1)]
    [InlineData(1000000, 100, 50)]
    public void Validate_WithBoundaryValues_ReturnsNoErrors(
        int monthlyDepositAmount,
        double expectedAnnualInterestRate,
        int timePeriodInYears)
    {
        // Arrange
        var input = new RecurringDepositInputViewModel
        {
            MonthlyDepositAmount = monthlyDepositAmount,
            ExpectedAnnualInterestRate = expectedAnnualInterestRate,
            TimePeriodInYears = timePeriodInYears
        };
        var validator = new RecurringDepositInputViewModelValidator();

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
        var input = new RecurringDepositInputViewModel
        {
            MonthlyDepositAmount = 5000,
            ExpectedAnnualInterestRate = 6.0,
            TimePeriodInYears = 5
        };
        var validator = new RecurringDepositInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WithZeroExpectedAnnualInterestRate_ReturnsNoErrors()
    {
        // Arrange - 0 is the valid lower bound for ExpectedAnnualInterestRate
        var input = new RecurringDepositInputViewModel
        {
            MonthlyDepositAmount = 5000,
            ExpectedAnnualInterestRate = 0,
            TimePeriodInYears = 5
        };
        var validator = new RecurringDepositInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    #endregion

    #region Negative Cases

    [Theory]
    [InlineData(999, 6.0, 5, nameof(RecurringDepositInputViewModel.MonthlyDepositAmount), "Monthly deposit amount must be between 1000 and 1000000.")]
    [InlineData(1000001, 6.0, 5, nameof(RecurringDepositInputViewModel.MonthlyDepositAmount), "Monthly deposit amount must be between 1000 and 1000000.")]
    [InlineData(5000, -1, 5, nameof(RecurringDepositInputViewModel.ExpectedAnnualInterestRate), "Expected annual interest rate must be between 0 and 100.")]
    [InlineData(5000, 101, 5, nameof(RecurringDepositInputViewModel.ExpectedAnnualInterestRate), "Expected annual interest rate must be between 0 and 100.")]
    [InlineData(5000, 6.0, 0, nameof(RecurringDepositInputViewModel.TimePeriodInYears), "Time period in years must be between 1 and 50.")]
    [InlineData(5000, 6.0, 51, nameof(RecurringDepositInputViewModel.TimePeriodInYears), "Time period in years must be between 1 and 50.")]
    public void Validate_WithInvalidField_ReturnsExpectedErrorMessage(
        int monthlyDepositAmount,
        double expectedAnnualInterestRate,
        int timePeriodInYears,
        string expectedPropertyName,
        string expectedMessage)
    {
        // Arrange
        var input = new RecurringDepositInputViewModel
        {
            MonthlyDepositAmount = monthlyDepositAmount,
            ExpectedAnnualInterestRate = expectedAnnualInterestRate,
            TimePeriodInYears = timePeriodInYears
        };
        var validator = new RecurringDepositInputViewModelValidator();

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
        var input = new RecurringDepositInputViewModel
        {
            MonthlyDepositAmount = 0,
            ExpectedAnnualInterestRate = -1,
            TimePeriodInYears = 0
        };
        var validator = new RecurringDepositInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(RecurringDepositInputViewModel.MonthlyDepositAmount) &&
            error.ErrorMessage == "Monthly deposit amount must be between 1000 and 1000000.");
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(RecurringDepositInputViewModel.ExpectedAnnualInterestRate) &&
            error.ErrorMessage == "Expected annual interest rate must be between 0 and 100.");
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(RecurringDepositInputViewModel.TimePeriodInYears) &&
            error.ErrorMessage == "Time period in years must be between 1 and 50.");
    }

    #endregion
}