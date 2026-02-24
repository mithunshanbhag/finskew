namespace FinSkew.Ui.UnitTests.Validators.InputModels;

public class SimpleInterestInputViewModelValidatorTests
{
    [Fact]
    public void Validate_WithValidInput_ReturnsNoErrors()
    {
        // Arrange
        var input = new SimpleInterestInputViewModel
        {
            PrincipalAmount = 10000,
            RateOfInterest = 5,
            TimePeriodInYears = 3
        };
        var validator = new SimpleInterestInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WithInvalidInput_ReturnsExpectedMessages()
    {
        // Arrange
        var input = new SimpleInterestInputViewModel
        {
            PrincipalAmount = 9999,
            RateOfInterest = 0.5,
            TimePeriodInYears = 0
        };
        var validator = new SimpleInterestInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(SimpleInterestInputViewModel.PrincipalAmount) &&
            error.ErrorMessage == "Principal amount must be between 10000 and 100000000.");
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(SimpleInterestInputViewModel.RateOfInterest) &&
            error.ErrorMessage == "Rate of interest must be between 1 and 100.");
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(SimpleInterestInputViewModel.TimePeriodInYears) &&
            error.ErrorMessage == "Time period in years must be between 1 and 100.");
    }
}