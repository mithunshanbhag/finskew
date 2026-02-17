using System.Globalization;

namespace FinSkew.Ui.UnitTests.Models.ViewModels.InputModels;

public class GratuityInputViewModelTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var viewModel = new GratuityInputViewModel();

        // Assert
        viewModel.Salary.Should().Be(50000);
        viewModel.YearsOfService.Should().Be(5);
    }

    [Theory]
    [InlineData(30000, 5)]
    [InlineData(60000, 10)]
    [InlineData(100000, 25)]
    public void Properties_ShouldBeSettable(int salary, int yearsOfService)
    {
        // Arrange
        var viewModel = new GratuityInputViewModel
        {
            Salary = salary,
            YearsOfService = yearsOfService
        };

        // Assert
        viewModel.Salary.Should().Be(salary);
        viewModel.YearsOfService.Should().Be(yearsOfService);
    }

    [Fact]
    public void Culture_ShouldBeIndianCulture()
    {
        // Act
        var viewModel = new GratuityInputViewModel();

        // Assert
        viewModel.Culture.Should().Be(CultureInfo.GetCultureInfo("en-In"));
    }
}