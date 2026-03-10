namespace FinSkew.Ui.UnitTests.Validators.InputModels;

public class CagrInputViewModelValidatorTests
{
    #region Positive Cases

    [Fact]
    public void Validate_WithValidInput_ReturnsNoErrors()
    {
        // Arrange
        var input = new CagrInputViewModel
        {
            InitialPrincipalAmount = 10000,
            FinalAmount = 15000,
            TimePeriodInYears = 3
        };
        var validator = new CagrInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    #endregion

    #region Boundary Cases

    [Theory]
    [InlineData(10000, 10000, 1)]
    [InlineData(100000000, 100000000, 100)]
    public void Validate_WithBoundaryValues_ReturnsNoErrors(
        int initialPrincipalAmount,
        int finalAmount,
        int timePeriodInYears)
    {
        // Arrange
        var input = new CagrInputViewModel
        {
            InitialPrincipalAmount = initialPrincipalAmount,
            FinalAmount = finalAmount,
            TimePeriodInYears = timePeriodInYears
        };
        var validator = new CagrInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion

    #region Negative Cases

    [Theory]
    [InlineData(9999, 15000, 3, nameof(CagrInputViewModel.InitialPrincipalAmount), "Invested amount must be between 10000 and 100000000.")]
    [InlineData(100000001, 15000, 3, nameof(CagrInputViewModel.InitialPrincipalAmount), "Invested amount must be between 10000 and 100000000.")]
    [InlineData(10000, 9999, 3, nameof(CagrInputViewModel.FinalAmount), "Final amount must be between 10000 and 100000000.")]
    [InlineData(10000, 100000001, 3, nameof(CagrInputViewModel.FinalAmount), "Final amount must be between 10000 and 100000000.")]
    [InlineData(10000, 15000, 0, nameof(CagrInputViewModel.TimePeriodInYears), "Time period in years must be between 1 and 100.")]
    [InlineData(10000, 15000, 101, nameof(CagrInputViewModel.TimePeriodInYears), "Time period in years must be between 1 and 100.")]
    public void Validate_WithInvalidField_ReturnsExpectedErrorMessage(
        int initialPrincipalAmount,
        int finalAmount,
        int timePeriodInYears,
        string expectedPropertyName,
        string expectedMessage)
    {
        // Arrange
        var input = new CagrInputViewModel
        {
            InitialPrincipalAmount = initialPrincipalAmount,
            FinalAmount = finalAmount,
            TimePeriodInYears = timePeriodInYears
        };
        var validator = new CagrInputViewModelValidator();

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
        var input = new CagrInputViewModel
        {
            InitialPrincipalAmount = 0,
            FinalAmount = 0,
            TimePeriodInYears = 0
        };
        var validator = new CagrInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(CagrInputViewModel.InitialPrincipalAmount) &&
            error.ErrorMessage == "Invested amount must be between 10000 and 100000000.");
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(CagrInputViewModel.FinalAmount) &&
            error.ErrorMessage == "Final amount must be between 10000 and 100000000.");
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(CagrInputViewModel.TimePeriodInYears) &&
            error.ErrorMessage == "Time period in years must be between 1 and 100.");
    }

    #endregion
}