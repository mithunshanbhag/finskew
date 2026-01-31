namespace FinSkew.Ui.UnitTests.Models.ViewModels.InputModels;

public class SimpleInterestInputViewModelTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var viewModel = new SimpleInterestInputViewModel();

        // Assert
        viewModel.PrincipalAmount.Should().Be(10000);
        viewModel.RateOfInterest.Should().Be(5.0);
        viewModel.TimePeriodInYears.Should().Be(3);
    }

    [Theory]
    [InlineData(10000, 5.0, 3)]
    [InlineData(50000, 8.5, 10)]
    [InlineData(100000, 12.0, 20)]
    public void Properties_ShouldBeSettable(int principal, double rate, int years)
    {
        // Arrange
        var viewModel = new SimpleInterestInputViewModel
        {
            // Act
            PrincipalAmount = principal,
            RateOfInterest = rate,
            TimePeriodInYears = years
        };

        // Assert
        viewModel.PrincipalAmount.Should().Be(principal);
        viewModel.RateOfInterest.Should().Be(rate);
        viewModel.TimePeriodInYears.Should().Be(years);
    }

    [Fact]
    public void PrincipalAmount_ShouldAcceptMinimumValue()
    {
        // Arrange
        var viewModel = new SimpleInterestInputViewModel
        {
            // Act
            PrincipalAmount = 10000
        };

        // Assert
        viewModel.PrincipalAmount.Should().Be(10000);
    }

    [Fact]
    public void PrincipalAmount_ShouldAcceptMaximumValue()
    {
        // Arrange
        var viewModel = new SimpleInterestInputViewModel
        {
            // Act
            PrincipalAmount = 100000000
        };

        // Assert
        viewModel.PrincipalAmount.Should().Be(100000000);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(5.5)]
    [InlineData(10.0)]
    [InlineData(50.0)]
    [InlineData(100.0)]
    public void RateOfInterest_ShouldAcceptValidRangeValues(double rate)
    {
        // Arrange
        var viewModel = new SimpleInterestInputViewModel
        {
            // Act
            RateOfInterest = rate
        };

        // Assert
        viewModel.RateOfInterest.Should().Be(rate);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    public void TimePeriodInYears_ShouldAcceptValidRangeValues(int years)
    {
        // Arrange
        var viewModel = new SimpleInterestInputViewModel
        {
            // Act
            TimePeriodInYears = years
        };

        // Assert
        viewModel.TimePeriodInYears.Should().Be(years);
    }

    [Fact]
    public void ViewModel_WithRandomValidValues_ShouldBeValid()
    {
        // Arrange
        var faker = new Faker();
        var viewModel = new SimpleInterestInputViewModel
        {
            // Act
            PrincipalAmount = faker.Random.Int(10000, 100000000),
            RateOfInterest = faker.Random.Double(1.0, 100.0),
            TimePeriodInYears = faker.Random.Int(1, 100)
        };

        // Assert
        viewModel.PrincipalAmount.Should().BeInRange(10000, 100000000);
        viewModel.RateOfInterest.Should().BeInRange(1.0, 100.0);
        viewModel.TimePeriodInYears.Should().BeInRange(1, 100);
    }
}