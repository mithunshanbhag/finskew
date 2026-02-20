namespace FinSkew.Ui.UnitTests.CalculationLogic;

public class EmiCalculationTests
{
    [Theory]
    [InlineData(100000, 8.5, 20, 867, 208277, 108277)]
    [InlineData(500000, 9.0, 15, 5071, 912839, 412839)]
    [InlineData(250000, 7.25, 10, 2935, 352203, 102203)]
    [InlineData(1200000, 0.0, 20, 5000, 1200000, 0)]
    public void CalculateResult_WithKnownInputs_ReturnsExpectedOutput(
        int principal,
        double annualRate,
        int tenureYears,
        int expectedMonthlyEmi,
        int expectedTotalPayment,
        int expectedTotalInterest)
    {
        // Arrange
        var input = new EmiInputViewModel
        {
            PrincipalAmount = principal,
            AnnualInterestRate = annualRate,
            LoanTenureInYears = tenureYears
        };

        // Act
        var result = CalculateEmi(input);

        // Assert
        result.Should().NotBeNull();
        result.Inputs.Should().Be(input);
        result.MonthlyEmi.Should().Be(expectedMonthlyEmi);
        result.TotalPayment.Should().Be(expectedTotalPayment);
        result.TotalInterest.Should().Be(expectedTotalInterest);
        result.YearlyGrowth.Should().HaveCount(tenureYears);
        result.YearlyGrowth.Last().Should().Be(expectedTotalPayment);
        result.YearlyGrowth.Should().BeInAscendingOrder();
    }

    [Theory]
    [InlineData(120000, 0.0, 10)]
    [InlineData(240000, 0.0, 20)]
    [InlineData(360000, 0.0, 30)]
    public void CalculateResult_ResultConsistency_TotalPaymentAndTotalInterestAreConsistent(
        int principal,
        double annualRate,
        int tenureYears)
    {
        // Arrange
        var input = new EmiInputViewModel
        {
            PrincipalAmount = principal,
            AnnualInterestRate = annualRate,
            LoanTenureInYears = tenureYears
        };
        var totalInstallments = input.LoanTenureInYears * 12;

        // Act
        var result = CalculateEmi(input);

        // Assert
        result.TotalPayment.Should().Be(result.MonthlyEmi * totalInstallments);
        result.TotalInterest.Should().Be(result.TotalPayment - result.Inputs.PrincipalAmount);
        result.YearlyGrowth.Should().HaveCount(input.LoanTenureInYears);
        result.YearlyGrowth.Should()
            .Equal(Enumerable.Range(1, input.LoanTenureInYears).Select(year => (principal / tenureYears) * year));
    }

    [Fact]
    public void CalculateResult_WithRandomInput_ComputedMonthlyEmiMatchesFormulaWithinTolerance()
    {
        // Arrange
        var faker = new Faker();
        var input = new EmiInputViewModel
        {
            PrincipalAmount = faker.Random.Int(10000, 1000000),
            AnnualInterestRate = faker.Random.Double(1.0, 15.0),
            LoanTenureInYears = faker.Random.Int(1, 30)
        };
        var monthlyInterestRate = input.AnnualInterestRate / (12d * 100d);
        var totalInstallments = input.LoanTenureInYears * 12;
        var expectedMonthlyEmi = input.PrincipalAmount * monthlyInterestRate * Math.Pow(1 + monthlyInterestRate, totalInstallments)
                                 / (Math.Pow(1 + monthlyInterestRate, totalInstallments) - 1);

        // Act
        var result = CalculateEmi(input);

        // Assert
        ((double)result.MonthlyEmi).Should().BeApproximately(expectedMonthlyEmi, 1.0);
        result.YearlyGrowth.Should().HaveCount(input.LoanTenureInYears);
        var expectedYearlyGrowth = Enumerable.Range(1, input.LoanTenureInYears)
            .Select(year => (int)(expectedMonthlyEmi * year * 12));
        result.YearlyGrowth.Should().Equal(expectedYearlyGrowth);
    }

    [Fact]
    public void CalculateResult_MultipleCallsWithSameInput_ReturnsSameResult()
    {
        // Arrange
        var input = new EmiInputViewModel
        {
            PrincipalAmount = 360000,
            AnnualInterestRate = 6.0,
            LoanTenureInYears = 30
        };

        // Act
        var result1 = CalculateEmi(input);
        var result2 = CalculateEmi(input);

        // Assert
        result1.MonthlyEmi.Should().Be(result2.MonthlyEmi);
        result1.TotalPayment.Should().Be(result2.TotalPayment);
        result1.TotalInterest.Should().Be(result2.TotalInterest);
        result1.YearlyGrowth.Should().Equal(result2.YearlyGrowth);
    }

    // Helper method that mimics the EMI calculator logic
    private static EmiResultViewModel CalculateEmi(EmiInputViewModel input)
    {
        EmiCalculator calculator = new();
        return calculator.Compute(input);
    }
}
