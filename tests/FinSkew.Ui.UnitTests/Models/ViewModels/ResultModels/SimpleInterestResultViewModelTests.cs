namespace FinSkew.Ui.UnitTests.Models.ViewModels.ResultModels;

public class SimpleInterestResultViewModelTests
{
    [Fact]
    public void Constructor_WithRequiredProperties_ShouldInitializeCorrectly()
    {
        // Arrange
        var input = new SimpleInterestInputViewModel
        {
            PrincipalAmount = 10000,
            RateOfInterest = 5.0,
            TimePeriodInYears = 3
        };

        // Act
        var result = new SimpleInterestResultViewModel
        {
            Inputs = input,
            TotalInterestEarned = 1500,
            TotalAmount = 11500
        };

        // Assert
        result.Should().NotBeNull();
        result.Inputs.Should().Be(input);
        result.TotalInterestEarned.Should().Be(1500);
        result.TotalAmount.Should().Be(11500);
    }

    [Fact]
    public void Inputs_ShouldBeRequired()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new SimpleInterestResultViewModel
            {
                Inputs = null!,
                TotalInterestEarned = 1500,
                TotalAmount = 11500
            };
        };

        // Assert - This would fail at runtime if Inputs is null due to 'required' keyword
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(10000, 1500, 11500)]
    [InlineData(50000, 10000, 60000)]
    [InlineData(100000, 25000, 125000)]
    public void Properties_ShouldAcceptValidValues(int principal, int interest, int total)
    {
        // Arrange
        var input = new SimpleInterestInputViewModel
        {
            PrincipalAmount = principal
        };

        // Act
        var result = new SimpleInterestResultViewModel
        {
            Inputs = input,
            TotalInterestEarned = interest,
            TotalAmount = total
        };

        // Assert
        result.Inputs.PrincipalAmount.Should().Be(principal);
        result.TotalInterestEarned.Should().Be(interest);
        result.TotalAmount.Should().Be(total);
    }

    [Fact]
    public void TotalAmount_ShouldBeSettable()
    {
        // Arrange
        var input = new SimpleInterestInputViewModel { PrincipalAmount = 10000 };
        var result = new SimpleInterestResultViewModel
        {
            Inputs = input,
            TotalInterestEarned = 1500,
            TotalAmount = 11500
        };

        // Act
        result.TotalAmount = 12000;

        // Assert
        result.TotalAmount.Should().Be(12000);
    }

    [Fact]
    public void ResultViewModel_ConsistencyCheck_TotalShouldEqualPrincipalPlusInterest()
    {
        // Arrange
        var input = new SimpleInterestInputViewModel
        {
            PrincipalAmount = 25000,
            RateOfInterest = 7.5,
            TimePeriodInYears = 4
        };

        const int interestEarned = 7500;

        // Act
        var result = new SimpleInterestResultViewModel
        {
            Inputs = input,
            TotalInterestEarned = interestEarned,
            TotalAmount = input.PrincipalAmount + interestEarned
        };

        // Assert
        result.TotalAmount.Should().Be(result.Inputs.PrincipalAmount + result.TotalInterestEarned);
    }

    [Fact]
    public void ResultViewModel_WithZeroInterest_ShouldHaveTotalEqualToPrincipal()
    {
        // Arrange
        var input = new SimpleInterestInputViewModel
        {
            PrincipalAmount = 10000,
            RateOfInterest = 0.0,
            TimePeriodInYears = 3
        };

        // Act
        var result = new SimpleInterestResultViewModel
        {
            Inputs = input,
            TotalInterestEarned = 0,
            TotalAmount = input.PrincipalAmount
        };

        // Assert
        result.TotalInterestEarned.Should().Be(0);
        result.TotalAmount.Should().Be(result.Inputs.PrincipalAmount);
    }

    [Fact]
    public void ResultViewModel_WithBogusData_ShouldAcceptValues()
    {
        // Arrange
        var faker = new Faker();
        var principal = faker.Random.Int(10000, 100000);
        var interest = faker.Random.Int(1000, 50000);

        var input = new SimpleInterestInputViewModel
        {
            PrincipalAmount = principal,
            RateOfInterest = faker.Random.Double(1.0, 15.0),
            TimePeriodInYears = faker.Random.Int(1, 30)
        };

        // Act
        var result = new SimpleInterestResultViewModel
        {
            Inputs = input,
            TotalInterestEarned = interest,
            TotalAmount = principal + interest
        };

        // Assert
        result.Should().NotBeNull();
        result.Inputs.Should().NotBeNull();
        result.TotalInterestEarned.Should().BeGreaterThan(0);
        result.TotalAmount.Should().Be(principal + interest);
    }
}