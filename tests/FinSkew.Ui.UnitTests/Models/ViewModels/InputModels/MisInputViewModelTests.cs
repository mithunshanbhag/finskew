namespace FinSkew.Ui.UnitTests.Models.ViewModels.InputModels;

public class MisInputViewModelTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithFixedDefaults()
    {
        // Act
        var viewModel = new MisInputViewModel();

        // Assert
        viewModel.InvestedAmount.Should().Be(100000);
        viewModel.AnnualInterestRate.Should().Be(6.6);
        viewModel.TimePeriodInYears.Should().Be(5);
    }

    [Theory]
    [InlineData(100000, 6.6)]
    [InlineData(250000, 7.25)]
    [InlineData(1000000, 9.5)]
    public void InvestedAmount_And_AnnualInterestRate_ShouldBeSettable(
        int investedAmount,
        double annualInterestRate)
    {
        // Arrange
        var viewModel = new MisInputViewModel
        {
            InvestedAmount = investedAmount,
            AnnualInterestRate = annualInterestRate
        };

        // Assert
        viewModel.InvestedAmount.Should().Be(investedAmount);
        viewModel.AnnualInterestRate.Should().Be(annualInterestRate);
    }

    [Fact]
    public void TimePeriodInYears_ShouldBeReadOnlyProperty()
    {
        // Act
        var timePeriodInYearsProperty = typeof(MisInputViewModel).GetProperty(nameof(MisInputViewModel.TimePeriodInYears));

        // Assert
        timePeriodInYearsProperty.Should().NotBeNull();
        timePeriodInYearsProperty!.CanWrite.Should().BeFalse();
    }
}
