namespace FinSkew.Ui.UnitTests.Validators.InputModels;

public class SwpInputViewModelValidatorTests
{
    #region Boundary Cases

    [Theory]
    [InlineData(10000, 500, 0, 1)]
    [InlineData(100000000, 10000000, 100, 50)]
    public void Validate_WithBoundaryValues_ReturnsNoErrors(
        int totalInvestmentAmount,
        int monthlyWithdrawalAmount,
        double expectedAnnualReturnRate,
        int timePeriodInYears)
    {
        // Arrange
        var input = new SwpInputViewModel
        {
            TotalInvestmentAmount = totalInvestmentAmount,
            MonthlyWithdrawalAmount = monthlyWithdrawalAmount,
            ExpectedAnnualReturnRate = expectedAnnualReturnRate,
            TimePeriodInYears = timePeriodInYears
        };
        var validator = new SwpInputViewModelValidator();

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
        var input = new SwpInputViewModel
        {
            TotalInvestmentAmount = 500000,
            MonthlyWithdrawalAmount = 10000,
            ExpectedAnnualReturnRate = 8.0,
            TimePeriodInYears = 5
        };
        var validator = new SwpInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WithZeroExpectedAnnualReturnRate_ReturnsNoErrors()
    {
        // Arrange - 0 is the valid lower bound for ExpectedAnnualReturnRate
        var input = new SwpInputViewModel
        {
            TotalInvestmentAmount = 500000,
            MonthlyWithdrawalAmount = 10000,
            ExpectedAnnualReturnRate = 0,
            TimePeriodInYears = 5
        };
        var validator = new SwpInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    #endregion

    #region Negative Cases

    [Theory]
    [InlineData(9999, 10000, 8.0, 5, nameof(SwpInputViewModel.TotalInvestmentAmount), "Invested amount must be between 10000 and 100000000.")]
    [InlineData(100000001, 10000, 8.0, 5, nameof(SwpInputViewModel.TotalInvestmentAmount), "Invested amount must be between 10000 and 100000000.")]
    [InlineData(500000, 499, 8.0, 5, nameof(SwpInputViewModel.MonthlyWithdrawalAmount), "Monthly withdrawal amount must be between 500 and 10000000.")]
    [InlineData(500000, 10000001, 8.0, 5, nameof(SwpInputViewModel.MonthlyWithdrawalAmount), "Monthly withdrawal amount must be between 500 and 10000000.")]
    [InlineData(500000, 10000, -1, 5, nameof(SwpInputViewModel.ExpectedAnnualReturnRate), "Expected annual return rate must be between 0 and 100.")]
    [InlineData(500000, 10000, 101, 5, nameof(SwpInputViewModel.ExpectedAnnualReturnRate), "Expected annual return rate must be between 0 and 100.")]
    [InlineData(500000, 10000, 8.0, 0, nameof(SwpInputViewModel.TimePeriodInYears), "Time period in years must be between 1 and 50.")]
    [InlineData(500000, 10000, 8.0, 51, nameof(SwpInputViewModel.TimePeriodInYears), "Time period in years must be between 1 and 50.")]
    public void Validate_WithInvalidField_ReturnsExpectedErrorMessage(
        int totalInvestmentAmount,
        int monthlyWithdrawalAmount,
        double expectedAnnualReturnRate,
        int timePeriodInYears,
        string expectedPropertyName,
        string expectedMessage)
    {
        // Arrange
        var input = new SwpInputViewModel
        {
            TotalInvestmentAmount = totalInvestmentAmount,
            MonthlyWithdrawalAmount = monthlyWithdrawalAmount,
            ExpectedAnnualReturnRate = expectedAnnualReturnRate,
            TimePeriodInYears = timePeriodInYears
        };
        var validator = new SwpInputViewModelValidator();

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
        var input = new SwpInputViewModel
        {
            TotalInvestmentAmount = 0,
            MonthlyWithdrawalAmount = 0,
            ExpectedAnnualReturnRate = -1,
            TimePeriodInYears = 0
        };
        var validator = new SwpInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(SwpInputViewModel.TotalInvestmentAmount) &&
            error.ErrorMessage == "Invested amount must be between 10000 and 100000000.");
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(SwpInputViewModel.MonthlyWithdrawalAmount) &&
            error.ErrorMessage == "Monthly withdrawal amount must be between 500 and 10000000.");
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(SwpInputViewModel.ExpectedAnnualReturnRate) &&
            error.ErrorMessage == "Expected annual return rate must be between 0 and 100.");
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(SwpInputViewModel.TimePeriodInYears) &&
            error.ErrorMessage == "Time period in years must be between 1 and 50.");
    }

    #endregion
}