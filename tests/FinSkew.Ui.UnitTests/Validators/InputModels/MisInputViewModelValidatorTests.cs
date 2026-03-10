namespace FinSkew.Ui.UnitTests.Validators.InputModels;

public class MisInputViewModelValidatorTests
{
    #region Positive Cases

    [Fact]
    public void Validate_WithValidInput_ReturnsNoErrors()
    {
        // Arrange
        var input = new MisInputViewModel
        {
            InvestedAmount = 100000,
            AnnualInterestRate = 6.6
            // TimePeriodInYears is a read-only property always equal to 5
        };
        var validator = new MisInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    #endregion

    #region Negative Cases

    [Theory]
    [InlineData(9999, 6.6, nameof(MisInputViewModel.InvestedAmount), "Invested amount must be between 10000 and 100000000.")]
    [InlineData(100000001, 6.6, nameof(MisInputViewModel.InvestedAmount), "Invested amount must be between 10000 and 100000000.")]
    [InlineData(100000, 0.5, nameof(MisInputViewModel.AnnualInterestRate), "Annual interest rate must be between 1 and 100.")]
    [InlineData(100000, 101, nameof(MisInputViewModel.AnnualInterestRate), "Annual interest rate must be between 1 and 100.")]
    public void Validate_WithInvalidField_ReturnsExpectedErrorMessage(
        int investedAmount,
        double annualInterestRate,
        string expectedPropertyName,
        string expectedMessage)
    {
        // Arrange
        var input = new MisInputViewModel
        {
            InvestedAmount = investedAmount,
            AnnualInterestRate = annualInterestRate
        };
        var validator = new MisInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == expectedPropertyName &&
            error.ErrorMessage == expectedMessage);
    }

    [Fact]
    public void Validate_WithAllInvalidWriteableFields_ReturnsAllErrorMessages()
    {
        // Arrange
        var input = new MisInputViewModel
        {
            InvestedAmount = 0,
            AnnualInterestRate = 0
        };
        var validator = new MisInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(MisInputViewModel.InvestedAmount) &&
            error.ErrorMessage == "Invested amount must be between 10000 and 100000000.");
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(MisInputViewModel.AnnualInterestRate) &&
            error.ErrorMessage == "Annual interest rate must be between 1 and 100.");
    }

    #endregion

    #region Boundary Cases

    [Theory]
    [InlineData(10000, 1)]
    [InlineData(100000000, 100)]
    public void Validate_WithBoundaryValues_ReturnsNoErrors(int investedAmount, double annualInterestRate)
    {
        // Arrange
        var input = new MisInputViewModel
        {
            InvestedAmount = investedAmount,
            AnnualInterestRate = annualInterestRate
        };
        var validator = new MisInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_TimePeriodInYears_IsAlwaysValidBecauseItIsReadOnly()
    {
        // The TimePeriodInYears property is read-only (always 5), matching the .Equal(5) rule.
        // This test documents that invariant so callers cannot accidentally set an invalid value.
        var input = new MisInputViewModel
        {
            InvestedAmount = 100000,
            AnnualInterestRate = 6.6
        };
        var validator = new MisInputViewModelValidator();

        var result = validator.Validate(input);

        result.Errors.Should().NotContain(error =>
            error.PropertyName == nameof(MisInputViewModel.TimePeriodInYears));
    }

    #endregion
}