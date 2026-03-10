namespace FinSkew.Ui.UnitTests.Validators.InputModels;

public class LumpsumInputViewModelValidatorTests
{
    #region Positive Cases

    [Fact]
    public void Validate_WithValidInput_ReturnsNoErrors()
    {
        // Arrange
        var input = new LumpsumInputViewModel
        {
            PrincipalAmount = 10000,
            RateOfInterest = 5,
            TimePeriodInYears = 3
        };
        var validator = new LumpsumInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    #endregion

    #region Boundary Cases

    [Theory]
    [InlineData(10000, 1, 1)]
    [InlineData(100000000, 100, 100)]
    public void Validate_WithBoundaryValues_ReturnsNoErrors(
        int principalAmount,
        double rateOfInterest,
        int timePeriodInYears)
    {
        // Arrange
        var input = new LumpsumInputViewModel
        {
            PrincipalAmount = principalAmount,
            RateOfInterest = rateOfInterest,
            TimePeriodInYears = timePeriodInYears
        };
        var validator = new LumpsumInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion

    #region Negative Cases

    [Theory]
    [InlineData(9999, 5, 3, nameof(LumpsumInputViewModel.PrincipalAmount), "Principal amount must be between 10000 and 100000000.")]
    [InlineData(100000001, 5, 3, nameof(LumpsumInputViewModel.PrincipalAmount), "Principal amount must be between 10000 and 100000000.")]
    [InlineData(10000, 0.5, 3, nameof(LumpsumInputViewModel.RateOfInterest), "Rate of interest must be between 1 and 100.")]
    [InlineData(10000, 101, 3, nameof(LumpsumInputViewModel.RateOfInterest), "Rate of interest must be between 1 and 100.")]
    [InlineData(10000, 5, 0, nameof(LumpsumInputViewModel.TimePeriodInYears), "Time period in years must be between 1 and 100.")]
    [InlineData(10000, 5, 101, nameof(LumpsumInputViewModel.TimePeriodInYears), "Time period in years must be between 1 and 100.")]
    public void Validate_WithInvalidField_ReturnsExpectedErrorMessage(
        int principalAmount,
        double rateOfInterest,
        int timePeriodInYears,
        string expectedPropertyName,
        string expectedMessage)
    {
        // Arrange
        var input = new LumpsumInputViewModel
        {
            PrincipalAmount = principalAmount,
            RateOfInterest = rateOfInterest,
            TimePeriodInYears = timePeriodInYears
        };
        var validator = new LumpsumInputViewModelValidator();

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
        var input = new LumpsumInputViewModel
        {
            PrincipalAmount = 0,
            RateOfInterest = 0,
            TimePeriodInYears = 0
        };
        var validator = new LumpsumInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(LumpsumInputViewModel.PrincipalAmount) &&
            error.ErrorMessage == "Principal amount must be between 10000 and 100000000.");
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(LumpsumInputViewModel.RateOfInterest) &&
            error.ErrorMessage == "Rate of interest must be between 1 and 100.");
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(LumpsumInputViewModel.TimePeriodInYears) &&
            error.ErrorMessage == "Time period in years must be between 1 and 100.");
    }

    #endregion
}