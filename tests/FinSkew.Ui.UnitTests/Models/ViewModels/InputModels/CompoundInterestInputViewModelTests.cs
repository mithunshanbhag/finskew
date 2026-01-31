namespace FinSkew.Ui.UnitTests.Models.ViewModels.InputModels;

public class CompoundInterestInputViewModelTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var viewModel = new CompoundInterestInputViewModel();

        // Assert
        viewModel.PrincipalAmount.Should().Be(10000);
        viewModel.RateOfInterest.Should().Be(5.0);
        viewModel.TimePeriodInYears.Should().Be(3);
        viewModel.CompoundingFrequencyPerYear.Should().Be(4);
    }

    [Theory]
    [InlineData(10000, 5.0, 3, 4)]
    [InlineData(50000, 8.5, 10, 12)]
    [InlineData(100000, 12.0, 20, 1)]
    public void Properties_ShouldBeSettable(int principal, double rate, int years, int frequency)
    {
        // Arrange
        var viewModel = new CompoundInterestInputViewModel
        {
            // Act
            PrincipalAmount = principal,
            RateOfInterest = rate,
            TimePeriodInYears = years,
            CompoundingFrequencyPerYear = frequency
        };

        // Assert
        viewModel.PrincipalAmount.Should().Be(principal);
        viewModel.RateOfInterest.Should().Be(rate);
        viewModel.TimePeriodInYears.Should().Be(years);
        viewModel.CompoundingFrequencyPerYear.Should().Be(frequency);
    }

    [Fact]
    public void PrincipalAmount_ShouldAcceptMinimumValue()
    {
        // Arrange
        var viewModel = new CompoundInterestInputViewModel
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
        var viewModel = new CompoundInterestInputViewModel
        {
            // Act
            PrincipalAmount = 100000000
        };

        // Assert
        viewModel.PrincipalAmount.Should().Be(100000000);
    }

    [Theory]
    [InlineData(1)] // Annual
    [InlineData(2)] // Semi-annual
    [InlineData(4)] // Quarterly
    [InlineData(12)] // Monthly
    [InlineData(365)] // Daily
    public void CompoundingFrequencyPerYear_ShouldAcceptCommonValues(int frequency)
    {
        // Arrange
        var viewModel = new CompoundInterestInputViewModel
        {
            // Act
            CompoundingFrequencyPerYear = frequency
        };

        // Assert
        viewModel.CompoundingFrequencyPerYear.Should().Be(frequency);
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
        var viewModel = new CompoundInterestInputViewModel
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
        var viewModel = new CompoundInterestInputViewModel
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
        var viewModel = new CompoundInterestInputViewModel
        {
            // Act
            PrincipalAmount = faker.Random.Int(10000, 100000000),
            RateOfInterest = faker.Random.Double(1.0, 100.0),
            TimePeriodInYears = faker.Random.Int(1, 100),
            CompoundingFrequencyPerYear = faker.PickRandom(1, 2, 4, 12, 365)
        };

        // Assert
        viewModel.PrincipalAmount.Should().BeInRange(10000, 100000000);
        viewModel.RateOfInterest.Should().BeInRange(1.0, 100.0);
        viewModel.TimePeriodInYears.Should().BeInRange(1, 100);
        viewModel.CompoundingFrequencyPerYear.Should().BeOneOf(1, 2, 4, 12, 365);
    }

    [Fact]
    public void CompoundInterestInputViewModel_ShouldHaveAllPropertiesOfSimpleInterest()
    {
        // Arrange
        var viewModel = new CompoundInterestInputViewModel();

        // Assert - verify it has all simple interest properties plus compounding frequency
        viewModel.Should().NotBeNull();
        viewModel.GetType().GetProperty(nameof(CompoundInterestInputViewModel.PrincipalAmount)).Should().NotBeNull();
        viewModel.GetType().GetProperty(nameof(CompoundInterestInputViewModel.RateOfInterest)).Should().NotBeNull();
        viewModel.GetType().GetProperty(nameof(CompoundInterestInputViewModel.TimePeriodInYears)).Should().NotBeNull();
        viewModel.GetType().GetProperty(nameof(CompoundInterestInputViewModel.CompoundingFrequencyPerYear)).Should()
            .NotBeNull();
    }
}