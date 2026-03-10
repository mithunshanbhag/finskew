namespace FinSkew.Ui.UnitTests.Validators.InputModels;

public class GratuityInputViewModelValidatorTests
{
    #region Positive Cases

    [Fact]
    public void Validate_WithValidInput_ReturnsNoErrors()
    {
        // Arrange
        var input = new GratuityInputViewModel
        {
            Salary = 50000,
            YearsOfService = 10
        };
        var validator = new GratuityInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    #endregion

    #region Boundary Cases

    [Theory]
    [InlineData(10000, 5)]
    [InlineData(100000000, 50)]
    public void Validate_WithBoundaryValues_ReturnsNoErrors(int salary, int yearsOfService)
    {
        // Arrange
        var input = new GratuityInputViewModel
        {
            Salary = salary,
            YearsOfService = yearsOfService
        };
        var validator = new GratuityInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion

    #region Negative Cases

    [Theory]
    [InlineData(9999, 10, nameof(GratuityInputViewModel.Salary), "Salary must be between 10000 and 100000000.")]
    [InlineData(100000001, 10, nameof(GratuityInputViewModel.Salary), "Salary must be between 10000 and 100000000.")]
    [InlineData(50000, 4, nameof(GratuityInputViewModel.YearsOfService), "Years of service must be between 5 and 50.")]
    [InlineData(50000, 51, nameof(GratuityInputViewModel.YearsOfService), "Years of service must be between 5 and 50.")]
    public void Validate_WithInvalidField_ReturnsExpectedErrorMessage(
        int salary,
        int yearsOfService,
        string expectedPropertyName,
        string expectedMessage)
    {
        // Arrange
        var input = new GratuityInputViewModel
        {
            Salary = salary,
            YearsOfService = yearsOfService
        };
        var validator = new GratuityInputViewModelValidator();

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
        var input = new GratuityInputViewModel
        {
            Salary = 0,
            YearsOfService = 0
        };
        var validator = new GratuityInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(GratuityInputViewModel.Salary) &&
            error.ErrorMessage == "Salary must be between 10000 and 100000000.");
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(GratuityInputViewModel.YearsOfService) &&
            error.ErrorMessage == "Years of service must be between 5 and 50.");
    }

    #endregion
}