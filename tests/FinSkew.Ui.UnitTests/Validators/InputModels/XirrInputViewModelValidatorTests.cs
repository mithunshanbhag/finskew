namespace FinSkew.Ui.UnitTests.Validators.InputModels;

public class XirrInputViewModelValidatorTests
{
    private static readonly DateTime ValidStartDate = DateTime.Today.AddYears(-5);
    private static readonly DateTime ValidEndDate = DateTime.Today;

    #region Positive Cases

    [Fact]
    public void Validate_WithValidInput_ReturnsNoErrors()
    {
        // Arrange
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = ValidStartDate,
            InvestmentMaturityDate = ValidEndDate,
            MonthlyInvestmentAmount = 1000,
            ExpectedAnnualReturnRate = 12.0
        };
        var validator = new XirrInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WithZeroExpectedAnnualReturnRate_ReturnsNoErrors()
    {
        // Arrange - 0 is a valid lower bound for ExpectedAnnualReturnRate
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = ValidStartDate,
            InvestmentMaturityDate = ValidEndDate,
            MonthlyInvestmentAmount = 1000,
            ExpectedAnnualReturnRate = 0
        };
        var validator = new XirrInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    #endregion

    #region Negative Cases

    [Fact]
    public void Validate_WhenStartDateIsAfterEndDate_ReturnsExpectedErrorMessages()
    {
        // Arrange
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = ValidEndDate,
            InvestmentMaturityDate = ValidStartDate,
            MonthlyInvestmentAmount = 1000,
            ExpectedAnnualReturnRate = 12.0
        };
        var validator = new XirrInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(XirrInputViewModel.InvestmentStartDate) &&
            error.ErrorMessage == "Investment start date must be before investment end date.");
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(XirrInputViewModel.InvestmentMaturityDate) &&
            error.ErrorMessage == "Investment end date must be after investment start date.");
    }

    [Fact]
    public void Validate_WhenStartDateIsTooFarInThePast_ReturnsExpectedErrorMessage()
    {
        // Arrange
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = DateTime.Today.AddYears(-101),
            InvestmentMaturityDate = ValidEndDate,
            MonthlyInvestmentAmount = 1000,
            ExpectedAnnualReturnRate = 12.0
        };
        var validator = new XirrInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(XirrInputViewModel.InvestmentStartDate) &&
            error.ErrorMessage == "Investment start date is out of allowed range.");
    }

    [Fact]
    public void Validate_WhenEndDateIsTooFarInTheFuture_ReturnsExpectedErrorMessage()
    {
        // Arrange
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = ValidStartDate,
            InvestmentMaturityDate = DateTime.Today.AddYears(101),
            MonthlyInvestmentAmount = 1000,
            ExpectedAnnualReturnRate = 12.0
        };
        var validator = new XirrInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(XirrInputViewModel.InvestmentMaturityDate) &&
            error.ErrorMessage == "Investment end date is out of allowed range.");
    }

    [Theory]
    [InlineData(499, 12.0, nameof(XirrInputViewModel.MonthlyInvestmentAmount), "Monthly investment amount must be between 500 and 10000000.")]
    [InlineData(10000001, 12.0, nameof(XirrInputViewModel.MonthlyInvestmentAmount), "Monthly investment amount must be between 500 and 10000000.")]
    [InlineData(1000, -1, nameof(XirrInputViewModel.ExpectedAnnualReturnRate), "Expected annual return rate must be between 0 and 100.")]
    [InlineData(1000, 101, nameof(XirrInputViewModel.ExpectedAnnualReturnRate), "Expected annual return rate must be between 0 and 100.")]
    public void Validate_WithInvalidAmountOrReturnRate_ReturnsExpectedErrorMessage(
        int monthlyInvestmentAmount,
        double expectedAnnualReturnRate,
        string expectedPropertyName,
        string expectedMessage)
    {
        // Arrange
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = ValidStartDate,
            InvestmentMaturityDate = ValidEndDate,
            MonthlyInvestmentAmount = monthlyInvestmentAmount,
            ExpectedAnnualReturnRate = expectedAnnualReturnRate
        };
        var validator = new XirrInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == expectedPropertyName &&
            error.ErrorMessage == expectedMessage);
    }

    #endregion

    #region Boundary Cases

    [Fact]
    public void Validate_WithStartDateExactlyAtLowerBound_ReturnsNoErrors()
    {
        // Arrange - exactly 100 years ago is the minimum allowed start date
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = DateTime.Today.AddYears(-100),
            InvestmentMaturityDate = DateTime.Today.AddYears(-99),
            MonthlyInvestmentAmount = 1000,
            ExpectedAnnualReturnRate = 12.0
        };
        var validator = new XirrInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithEndDateExactlyAtUpperBound_ReturnsNoErrors()
    {
        // Arrange - exactly 100 years from now is the maximum allowed maturity date
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = ValidStartDate,
            InvestmentMaturityDate = DateTime.Today.AddYears(100),
            MonthlyInvestmentAmount = 1000,
            ExpectedAnnualReturnRate = 12.0
        };
        var validator = new XirrInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(500, 0)]
    [InlineData(10000000, 100)]
    public void Validate_WithBoundaryAmountAndReturnRate_ReturnsNoErrors(
        int monthlyInvestmentAmount,
        double expectedAnnualReturnRate)
    {
        // Arrange
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = ValidStartDate,
            InvestmentMaturityDate = ValidEndDate,
            MonthlyInvestmentAmount = monthlyInvestmentAmount,
            ExpectedAnnualReturnRate = expectedAnnualReturnRate
        };
        var validator = new XirrInputViewModelValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion
}