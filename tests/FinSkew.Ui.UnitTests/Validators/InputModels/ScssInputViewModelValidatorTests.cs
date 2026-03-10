namespace FinSkew.Ui.UnitTests.Validators.InputModels;

public class ScssInputViewModelValidatorTests
{
    #region Positive Cases

    [Fact]
    public void Validate_WithValidInput_ReturnsNoErrors()
    {
        // Arrange
        var input = new ScssInputViewModel
        {
            PrincipalAmount = 100000
            // AnnualInterestRate is read-only (always 7.4) — always passes its .Equal(7.4) rule
            // TenureInYears is read-only (always 5) — always passes its .Equal(5) rule
        };
        var validator = new ScssInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    #endregion

    #region Negative Cases

    [Theory]
    [InlineData(9999, "Principal amount must be between 10000 and 100000000.")]
    [InlineData(100000001, "Principal amount must be between 10000 and 100000000.")]
    public void Validate_WithInvalidPrincipalAmount_ReturnsExpectedErrorMessage(
        int principalAmount,
        string expectedMessage)
    {
        // Arrange
        var input = new ScssInputViewModel
        {
            PrincipalAmount = principalAmount
        };
        var validator = new ScssInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(ScssInputViewModel.PrincipalAmount) &&
            error.ErrorMessage == expectedMessage);
    }

    #endregion

    #region Boundary Cases

    [Theory]
    [InlineData(10000)]
    [InlineData(100000000)]
    public void Validate_WithBoundaryPrincipalAmount_ReturnsNoErrors(int principalAmount)
    {
        // Arrange
        var input = new ScssInputViewModel
        {
            PrincipalAmount = principalAmount
        };
        var validator = new ScssInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_AnnualInterestRateAndTenureInYears_AreAlwaysValidBecauseTheyAreReadOnly()
    {
        // AnnualInterestRate is read-only (always 7.4) and TenureInYears is read-only (always 5),
        // so their validators (.Equal(7.4) and .Equal(5)) can never fail.
        var input = new ScssInputViewModel
        {
            PrincipalAmount = 100000
        };
        var validator = new ScssInputViewModelValidator();

        var result = validator.Validate(input);

        result.Errors.Should().NotContain(error =>
            error.PropertyName == nameof(ScssInputViewModel.AnnualInterestRate));
        result.Errors.Should().NotContain(error =>
            error.PropertyName == nameof(ScssInputViewModel.TenureInYears));
    }

    #endregion
}