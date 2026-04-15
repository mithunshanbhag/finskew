namespace FinSkew.Ui.UnitTests.Models.ViewModels.ResultModels;

public class CompoundInterestResultViewModelTests
{
    #region Negative cases

    [Fact]
    public void Inputs_ShouldAcceptNullSuppressionAtRuntime()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new CompoundInterestResultViewModel
            {
                Inputs = null!,
                TotalInterestEarned = 1608,
                TotalAmount = 11608,
                YearlyGrowth = [10500, 11025, 11608]
            };
        };

        // Assert
        act.Should().NotThrow();
    }

    #endregion

    #region Boundary cases

    [Fact]
    public void ResultViewModel_WithZeroInterest_ShouldHaveTotalEqualToPrincipal()
    {
        // Arrange
        var input = CreateInput(10000, 0.0, 3, 1);

        // Act
        var result = new CompoundInterestResultViewModel
        {
            Inputs = input,
            TotalInterestEarned = 0,
            TotalAmount = input.PrincipalAmount,
            YearlyGrowth = [10000, 10000, 10000]
        };

        // Assert
        result.TotalInterestEarned.Should().Be(0);
        result.TotalAmount.Should().Be(result.Inputs.PrincipalAmount);
        result.YearlyGrowthAsStr.Should().Equal("₹10,000", "₹10,000", "₹10,000");
    }

    #endregion

    private static CompoundInterestInputViewModel CreateInput(
        int principalAmount = 10000,
        double rateOfInterest = 5.0,
        int timePeriodInYears = 3,
        int compoundingFrequencyPerYear = 4)
    {
        return new CompoundInterestInputViewModel
        {
            PrincipalAmount = principalAmount,
            RateOfInterest = rateOfInterest,
            TimePeriodInYears = timePeriodInYears,
            CompoundingFrequencyPerYear = compoundingFrequencyPerYear
        };
    }

    #region Positive cases

    [Fact]
    public void Constructor_WithRequiredProperties_ShouldInitializeCorrectly()
    {
        // Arrange
        var input = CreateInput();

        // Act
        var result = new CompoundInterestResultViewModel
        {
            Inputs = input,
            TotalInterestEarned = 1608,
            TotalAmount = 11608,
            YearlyGrowth = [10500, 11025, 11608]
        };

        // Assert
        result.Should().NotBeNull();
        result.Inputs.Should().Be(input);
        result.TotalInterestEarned.Should().Be(1608);
        result.TotalAmount.Should().Be(11608);
        result.YearlyGrowth.Should().Equal(10500, 11025, 11608);
    }

    [Theory]
    [InlineData(10000, 1608, 11608, new[] { 10500, 11025, 11608 })]
    [InlineData(50000, 12167, 62167, new[] { 53500, 57325, 62167 })]
    [InlineData(100000, 34010, 134010, new[] { 110250, 121551, 134010 })]
    public void Properties_ShouldAcceptValidValues(int principal, int interest, int total, int[] yearlyGrowth)
    {
        // Arrange
        var input = CreateInput(principal);

        // Act
        var result = new CompoundInterestResultViewModel
        {
            Inputs = input,
            TotalInterestEarned = interest,
            TotalAmount = total,
            YearlyGrowth = yearlyGrowth
        };

        // Assert
        result.Inputs.PrincipalAmount.Should().Be(principal);
        result.TotalInterestEarned.Should().Be(interest);
        result.TotalAmount.Should().Be(total);
        result.YearlyGrowth.Should().Equal(yearlyGrowth);
    }

    [Fact]
    public void ResultViewModel_ConsistencyCheck_TotalShouldEqualPrincipalPlusInterest()
    {
        // Arrange
        var input = CreateInput(25000, 8.0, 4, 1);
        const int totalInterestEarned = 9024;

        // Act
        var result = new CompoundInterestResultViewModel
        {
            Inputs = input,
            TotalInterestEarned = totalInterestEarned,
            TotalAmount = input.PrincipalAmount + totalInterestEarned,
            YearlyGrowth = [27000, 29160, 31493, 34024]
        };

        // Assert
        result.TotalAmount.Should().Be(result.Inputs.PrincipalAmount + result.TotalInterestEarned);
    }

    [Fact]
    public void ResultViewModel_WithBogusData_ShouldAcceptValues()
    {
        // Arrange
        var faker = new Faker();
        var principal = faker.Random.Int(10000, 100000);
        var interest = faker.Random.Int(1000, 50000);
        var total = principal + interest;

        var input = CreateInput(
            principal,
            faker.Random.Double(1.0, 15.0),
            faker.Random.Int(1, 30),
            faker.PickRandom(1, 2, 4, 12));

        // Act
        var result = new CompoundInterestResultViewModel
        {
            Inputs = input,
            TotalInterestEarned = interest,
            TotalAmount = total,
            YearlyGrowth = [total]
        };

        // Assert
        result.Should().NotBeNull();
        result.Inputs.Should().NotBeNull();
        result.TotalInterestEarned.Should().BeGreaterThan(0);
        result.TotalAmount.Should().Be(total);
        result.YearlyGrowth.Should().Equal(total);
    }

    [Theory]
    [InlineData(10000, 11608, 1608, "₹10,000", "₹11,608", "₹1,608")]
    [InlineData(50000, 62167, 12167, "₹50,000", "₹62,167", "₹12,167")]
    public void ComputedCurrencyHelpers_ShouldFormatUsingInputCulture(
        int principal,
        int totalAmount,
        int totalInterestEarned,
        string expectedPrincipal,
        string expectedTotalAmount,
        string expectedTotalInterestEarned)
    {
        // Arrange
        var input = CreateInput(principal);
        var result = new CompoundInterestResultViewModel
        {
            Inputs = input,
            TotalInterestEarned = totalInterestEarned,
            TotalAmount = totalAmount,
            YearlyGrowth = [totalAmount]
        };

        // Act & Assert
        result.PrincipalAmountStr.Should().Be(expectedPrincipal);
        result.TotalAmountStr.Should().Be(expectedTotalAmount);
        result.TotalInterestEarnedStr.Should().Be(expectedTotalInterestEarned);
    }

    [Fact]
    public void YearlyGrowthAsStr_ShouldFormatYearlyGrowthUsingInputCulture()
    {
        // Arrange
        var input = CreateInput();
        var result = new CompoundInterestResultViewModel
        {
            Inputs = input,
            TotalInterestEarned = 1608,
            TotalAmount = 11608,
            YearlyGrowth = [10500, 11025, 11608]
        };

        // Act & Assert
        result.YearlyGrowthAsStr.Should().Equal("₹10,500", "₹11,025", "₹11,608");
    }

    #endregion
}