namespace FinSkew.Ui.UnitTests.Models.ViewModels.ResultModels;

public class GratuityResultViewModelTests
{
    [Fact]
    public void Constructor_WithRequiredProperties_ShouldInitializeCorrectly()
    {
        // Arrange
        var input = new GratuityInputViewModel
        {
            Salary = 50000,
            YearsOfService = 5
        };

        // Act
        var result = new GratuityResultViewModel
        {
            Inputs = input,
            TotalSalaryDrawn = 3000000,
            GratuityAmount = 144230
        };

        // Assert
        result.Should().NotBeNull();
        result.Inputs.Should().Be(input);
        result.TotalSalaryDrawn.Should().Be(3000000);
        result.GratuityAmount.Should().Be(144230);
    }

    [Fact]
    public void GratuityAmountStr_ShouldFormatUsingInputCulture()
    {
        // Arrange
        var input = new GratuityInputViewModel
        {
            Salary = 50000,
            YearsOfService = 5
        };

        var result = new GratuityResultViewModel
        {
            Inputs = input,
            TotalSalaryDrawn = 3000000,
            GratuityAmount = 144230
        };

        // Act
        var formattedValue = result.GratuityAmountStr;

        // Assert
        formattedValue.Should().Be(result.GratuityAmount.ToString("C0", input.Culture));
    }

    [Fact]
    public void TotalSalaryDrawnStr_ShouldFormatUsingInputCulture()
    {
        // Arrange
        var input = new GratuityInputViewModel
        {
            Salary = 50000,
            YearsOfService = 5
        };

        var result = new GratuityResultViewModel
        {
            Inputs = input,
            TotalSalaryDrawn = 3000000,
            GratuityAmount = 144230
        };

        // Act
        var formattedValue = result.TotalSalaryDrawnStr;

        // Assert
        formattedValue.Should().Be(result.TotalSalaryDrawn.ToString("C0", input.Culture));
    }
}
