namespace FinSkew.Ui.UnitTests.Models.ViewModels.InputModels;

public class ScssInputViewModelTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithFixedDefaults()
    {
        // Act
        var viewModel = new ScssInputViewModel();

        // Assert
        viewModel.PrincipalAmount.Should().Be(10000);
        viewModel.AnnualInterestRate.Should().Be(7.4);
        viewModel.TenureInYears.Should().Be(5);
    }

    [Theory]
    [InlineData(10000)]
    [InlineData(50000)]
    [InlineData(100000000)]
    public void PrincipalAmount_ShouldBeSettable(int principalAmount)
    {
        // Arrange
        var viewModel = new ScssInputViewModel
        {
            PrincipalAmount = principalAmount
        };

        // Assert
        viewModel.PrincipalAmount.Should().Be(principalAmount);
    }

    [Fact]
    public void AnnualInterestRate_And_TenureInYears_ShouldBeReadOnlyProperties()
    {
        // Act
        var annualInterestRateProperty = typeof(ScssInputViewModel).GetProperty(nameof(ScssInputViewModel.AnnualInterestRate));
        var tenureInYearsProperty = typeof(ScssInputViewModel).GetProperty(nameof(ScssInputViewModel.TenureInYears));

        // Assert
        annualInterestRateProperty.Should().NotBeNull();
        annualInterestRateProperty!.CanWrite.Should().BeFalse();
        tenureInYearsProperty.Should().NotBeNull();
        tenureInYearsProperty!.CanWrite.Should().BeFalse();
    }
}
