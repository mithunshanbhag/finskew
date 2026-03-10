namespace FinSkew.Ui.UnitTests.Validators.InputModels;

public class EmiInputViewModelValidatorTests
{
    #region Boundary Cases

    [Theory]
    [InlineData(10000, 0, 1)]
    [InlineData(100000000, 100, 50)]
    public void Validate_WithBoundaryValues_ReturnsNoErrors(
        int principalAmount,
        double annualInterestRate,
        int loanTenureInYears)
    {
        // Arrange
        var input = new EmiInputViewModel
        {
            PrincipalAmount = principalAmount,
            AnnualInterestRate = annualInterestRate,
            LoanTenureInYears = loanTenureInYears
        };
        var validator = new EmiInputViewModelValidator();

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
        var input = new EmiInputViewModel
        {
            PrincipalAmount = 100000,
            AnnualInterestRate = 8.5,
            LoanTenureInYears = 20
        };
        var validator = new EmiInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WithZeroAnnualInterestRate_ReturnsNoErrors()
    {
        // Arrange - 0 is a valid lower bound for AnnualInterestRate
        var input = new EmiInputViewModel
        {
            PrincipalAmount = 100000,
            AnnualInterestRate = 0,
            LoanTenureInYears = 10
        };
        var validator = new EmiInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    #endregion

    #region Negative Cases

    [Theory]
    [InlineData(9999, 8.5, 20, nameof(EmiInputViewModel.PrincipalAmount), "Loan amount must be between 10000 and 100000000.")]
    [InlineData(100000001, 8.5, 20, nameof(EmiInputViewModel.PrincipalAmount), "Loan amount must be between 10000 and 100000000.")]
    [InlineData(100000, -1, 20, nameof(EmiInputViewModel.AnnualInterestRate), "Annual interest rate must be between 0 and 100.")]
    [InlineData(100000, 101, 20, nameof(EmiInputViewModel.AnnualInterestRate), "Annual interest rate must be between 0 and 100.")]
    [InlineData(100000, 8.5, 0, nameof(EmiInputViewModel.LoanTenureInYears), "Loan tenure in years must be between 1 and 50.")]
    [InlineData(100000, 8.5, 51, nameof(EmiInputViewModel.LoanTenureInYears), "Loan tenure in years must be between 1 and 50.")]
    public void Validate_WithInvalidField_ReturnsExpectedErrorMessage(
        int principalAmount,
        double annualInterestRate,
        int loanTenureInYears,
        string expectedPropertyName,
        string expectedMessage)
    {
        // Arrange
        var input = new EmiInputViewModel
        {
            PrincipalAmount = principalAmount,
            AnnualInterestRate = annualInterestRate,
            LoanTenureInYears = loanTenureInYears
        };
        var validator = new EmiInputViewModelValidator();

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
        var input = new EmiInputViewModel
        {
            PrincipalAmount = 0,
            AnnualInterestRate = -1,
            LoanTenureInYears = 0
        };
        var validator = new EmiInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(EmiInputViewModel.PrincipalAmount) &&
            error.ErrorMessage == "Loan amount must be between 10000 and 100000000.");
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(EmiInputViewModel.AnnualInterestRate) &&
            error.ErrorMessage == "Annual interest rate must be between 0 and 100.");
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(EmiInputViewModel.LoanTenureInYears) &&
            error.ErrorMessage == "Loan tenure in years must be between 1 and 50.");
    }

    #endregion
}