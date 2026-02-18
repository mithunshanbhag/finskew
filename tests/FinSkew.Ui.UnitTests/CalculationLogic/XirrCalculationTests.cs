namespace FinSkew.Ui.UnitTests.CalculationLogic;

public class XirrCalculationTests
{
    [Theory]
    [InlineData("2020-01-01", "2025-01-01", 10000, 12.0, 12.66)]
    [InlineData("2020-01-01", "2025-01-01", 5000, 10.0, 10.45)]
    [InlineData("2020-01-01", "2030-01-01", 1000, 15.0, 16.06)]
    [InlineData("2020-01-01", "2023-01-01", 25000, 8.0, 8.34)]
    public void Compute_WithValidInputs_ReturnsExpectedXirr(
        string startDate,
        string maturityDate,
        int monthlyAmount,
        double expectedReturnRate,
        double expectedXirr)
    {
        // Arrange
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = DateTime.Parse(startDate),
            InvestmentMaturityDate = DateTime.Parse(maturityDate),
            MonthlyInvestmentAmount = monthlyAmount,
            ExpectedAnnualReturnRate = expectedReturnRate
        };

        // Act
        var result = CalculateXirr(input);

        // Assert
        result.Should().NotBeNull();
        result.Inputs.Should().Be(input);
        result.Xirr.Should().BeApproximately(expectedXirr / 100, 0.01);

        var cashflows = BuildTestCashflows(input);
        var expectedInitialPrincipal = cashflows.Where(cashflow => cashflow.Amount < 0).Sum(cashflow => -cashflow.Amount);
        var expectedFinalAmount = cashflows[^1].Amount;
        result.InitialPrincipal.Should().Be(expectedInitialPrincipal);
        result.FinalAmount.Should().BeApproximately(expectedFinalAmount, 1e-6);
        result.TotalGain.Should().BeApproximately(expectedFinalAmount - expectedInitialPrincipal, 1e-6);
    }

    [Fact]
    public void Compute_WithZeroReturnRate_ReturnsNearZeroXirr()
    {
        // Arrange
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = new DateTime(2020, 1, 1),
            InvestmentMaturityDate = new DateTime(2025, 1, 1),
            MonthlyInvestmentAmount = 10000,
            ExpectedAnnualReturnRate = 0.0
        };

        // Act
        var result = CalculateXirr(input);

        // Assert
        result.Xirr.Should().BeApproximately(0.0, 0.01);
    }

    [Theory]
    [InlineData("2020-01-01", "2021-01-01", 1000, 12.0)]
    [InlineData("2020-01-01", "2022-01-01", 1000, 12.0)]
    [InlineData("2020-01-01", "2025-01-01", 1000, 12.0)]
    [InlineData("2020-01-01", "2030-01-01", 1000, 12.0)]
    public void Compute_DifferentDateSpans_ReturnsXirrCloseToExpectedRate(
        string startDate,
        string maturityDate,
        int monthlyAmount,
        double expectedReturnRate)
    {
        // Arrange
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = DateTime.Parse(startDate),
            InvestmentMaturityDate = DateTime.Parse(maturityDate),
            MonthlyInvestmentAmount = monthlyAmount,
            ExpectedAnnualReturnRate = expectedReturnRate
        };

        // Act
        var result = CalculateXirr(input);

        // Assert - XIRR should be close to expected return rate
        // but slightly higher due to annuity due effect (monthly compounding)
        result.Xirr.Should().BeApproximately(expectedReturnRate / 100, 0.05);
        result.Xirr.Should().BeGreaterOrEqualTo(expectedReturnRate / 100 - 0.001);
    }

    [Theory]
    [InlineData("2020-01-01", "2020-02-01")]
    [InlineData("2020-01-01", "2020-03-01")]
    [InlineData("2020-01-01", "2020-06-01")]
    public void Compute_ShortDateSpan_HandlesEdgeCaseCorrectly(
        string startDate,
        string maturityDate)
    {
        // Arrange
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = DateTime.Parse(startDate),
            InvestmentMaturityDate = DateTime.Parse(maturityDate),
            MonthlyInvestmentAmount = 10000,
            ExpectedAnnualReturnRate = 12.0
        };

        // Act
        var result = CalculateXirr(input);

        // Assert - Should complete without error and produce reasonable result
        result.Should().NotBeNull();
        result.Xirr.Should().BeGreaterThanOrEqualTo(-1.0);
        result.Xirr.Should().BeLessThan(5.0);
    }

    [Fact]
    public void Compute_SingleMonthSpan_ReturnsValidResult()
    {
        // Arrange - Only one cashflow investment, maturity on next month
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = new DateTime(2020, 1, 1),
            InvestmentMaturityDate = new DateTime(2020, 2, 1),
            MonthlyInvestmentAmount = 10000,
            ExpectedAnnualReturnRate = 12.0
        };

        // Act
        var result = CalculateXirr(input);

        // Assert
        result.Should().NotBeNull();
        result.Xirr.Should().BeGreaterThanOrEqualTo(-0.99);
    }

    [Theory]
    [InlineData("2020-01-01", "2050-01-01", 1000, 12.0)]
    [InlineData("2020-01-01", "2060-01-01", 5000, 10.0)]
    public void Compute_VeryLongDateSpan_HandlesCorrectly(
        string startDate,
        string maturityDate,
        int monthlyAmount,
        double expectedReturnRate)
    {
        // Arrange
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = DateTime.Parse(startDate),
            InvestmentMaturityDate = DateTime.Parse(maturityDate),
            MonthlyInvestmentAmount = monthlyAmount,
            ExpectedAnnualReturnRate = expectedReturnRate
        };

        // Act
        var result = CalculateXirr(input);

        // Assert
        result.Should().NotBeNull();
        result.Xirr.Should().BeApproximately(expectedReturnRate / 100, 0.05);
    }

    [Fact]
    public void Compute_WithMaturityBeforeStart_ReturnsZeroXirr()
    {
        // Arrange
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = new DateTime(2025, 1, 1),
            InvestmentMaturityDate = new DateTime(2020, 1, 1),
            MonthlyInvestmentAmount = 10000,
            ExpectedAnnualReturnRate = 12.0
        };

        // Act
        var result = CalculateXirr(input);

        // Assert
        result.Xirr.Should().Be(0.0);
        result.InitialPrincipal.Should().Be(0.0);
        result.TotalGain.Should().Be(0.0);
        result.FinalAmount.Should().Be(0.0);
    }

    [Fact]
    public void Compute_WithMaturityEqualToStart_ReturnsZeroXirr()
    {
        // Arrange
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = new DateTime(2020, 1, 1),
            InvestmentMaturityDate = new DateTime(2020, 1, 1),
            MonthlyInvestmentAmount = 10000,
            ExpectedAnnualReturnRate = 12.0
        };

        // Act
        var result = CalculateXirr(input);

        // Assert
        result.Xirr.Should().Be(0.0);
        result.InitialPrincipal.Should().Be(0.0);
        result.TotalGain.Should().Be(0.0);
        result.FinalAmount.Should().Be(0.0);
    }

    [Theory]
    [InlineData(500, 12.0)]
    [InlineData(100000, 12.0)]
    [InlineData(1000000, 15.0)]
    public void Compute_DifferentInvestmentAmounts_ProducesConsistentXirr(
        int monthlyAmount,
        double expectedReturnRate)
    {
        // Arrange
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = new DateTime(2020, 1, 1),
            InvestmentMaturityDate = new DateTime(2025, 1, 1),
            MonthlyInvestmentAmount = monthlyAmount,
            ExpectedAnnualReturnRate = expectedReturnRate
        };

        // Act
        var result = CalculateXirr(input);

        // Assert - XIRR should be consistent regardless of amount scale
        result.Xirr.Should().BeApproximately(expectedReturnRate / 100, 0.05);
    }

    [Theory]
    [InlineData(0.5)]
    [InlineData(5.0)]
    [InlineData(10.0)]
    [InlineData(15.0)]
    [InlineData(20.0)]
    public void Compute_DifferentReturnRates_ProducesXirrCloseToExpected(double expectedReturnRate)
    {
        // Arrange
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = new DateTime(2020, 1, 1),
            InvestmentMaturityDate = new DateTime(2025, 1, 1),
            MonthlyInvestmentAmount = 10000,
            ExpectedAnnualReturnRate = expectedReturnRate
        };

        // Act
        var result = CalculateXirr(input);

        // Assert - XIRR will be slightly higher due to monthly compounding effect
        result.Xirr.Should().BeApproximately(expectedReturnRate / 100, 0.05);
        result.Xirr.Should().BeGreaterOrEqualTo(expectedReturnRate / 100 - 0.001);
    }

    [Fact]
    public void Compute_MultipleCallsWithSameInput_ReturnsSameResult()
    {
        // Arrange
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = new DateTime(2020, 1, 1),
            InvestmentMaturityDate = new DateTime(2025, 1, 1),
            MonthlyInvestmentAmount = 10000,
            ExpectedAnnualReturnRate = 12.0
        };

        // Act
        var result1 = CalculateXirr(input);
        var result2 = CalculateXirr(input);

        // Assert
        result1.Xirr.Should().Be(result2.Xirr);
    }

    [Fact]
    public void Compute_ResultConsistency_XirrSatisfiesNpvEquation()
    {
        // Arrange
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = new DateTime(2020, 1, 1),
            InvestmentMaturityDate = new DateTime(2025, 1, 1),
            MonthlyInvestmentAmount = 10000,
            ExpectedAnnualReturnRate = 12.0
        };

        // Act
        var result = CalculateXirr(input);

        // Assert - Verify NPV equation is satisfied
        var cashflows = BuildTestCashflows(input);
        var npv = ComputeNpv(cashflows, input.InvestmentStartDate, result.Xirr);
        npv.Should().BeApproximately(0.0, 1e-6);

        result.InitialPrincipal.Should().Be(cashflows.Where(cashflow => cashflow.Amount < 0).Sum(cashflow => -cashflow.Amount));
        result.FinalAmount.Should().BeApproximately(cashflows[^1].Amount, 1e-6);
        result.TotalGain.Should().BeApproximately(result.FinalAmount - result.InitialPrincipal, 1e-6);
    }

    [Fact]
    public void Compute_ResultConsistency_XirrWithRandomInputs()
    {
        // Arrange
        var faker = new Faker();
        var startDate = new DateTime(2020, 1, 1);
        var yearsToMaturity = faker.Random.Int(1, 20);
        var maturityDate = startDate.AddYears(yearsToMaturity);

        var input = new XirrInputViewModel
        {
            InvestmentStartDate = startDate,
            InvestmentMaturityDate = maturityDate,
            MonthlyInvestmentAmount = faker.Random.Int(1000, 100000),
            ExpectedAnnualReturnRate = faker.Random.Double(1.0, 30.0)
        };

        // Act
        var result = CalculateXirr(input);

        // Assert - Verify NPV equation is satisfied
        var cashflows = BuildTestCashflows(input);
        var npv = ComputeNpv(cashflows, input.InvestmentStartDate, result.Xirr);
        npv.Should().BeApproximately(0.0, 1e-4);
    }

    [Fact]
    public void Compute_XirrStringFormat_IncludesPercentageAndTwoDecimals()
    {
        // Arrange
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = new DateTime(2020, 1, 1),
            InvestmentMaturityDate = new DateTime(2025, 1, 1),
            MonthlyInvestmentAmount = 10000,
            ExpectedAnnualReturnRate = 12.0
        };

        // Act
        var result = CalculateXirr(input);

        // Assert
        result.InitialPrincipalStr.Should().Contain("₹");
        result.TotalGainStr.Should().Contain("₹");
        result.FinalAmountStr.Should().Contain("₹");
        result.XirrStr.Should().Contain("%");
        result.XirrStr.Should().MatchRegex(@"\d+\.\d{2}%");
    }

    [Theory]
    [InlineData("2020-01-15", "2025-01-15")]
    [InlineData("2020-03-31", "2025-03-31")]
    [InlineData("2020-06-30", "2025-06-30")]
    public void Compute_DifferentStartDaysOfMonth_ProducesConsistentResults(
        string startDate,
        string maturityDate)
    {
        // Arrange
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = DateTime.Parse(startDate),
            InvestmentMaturityDate = DateTime.Parse(maturityDate),
            MonthlyInvestmentAmount = 10000,
            ExpectedAnnualReturnRate = 12.0
        };

        // Act
        var result = CalculateXirr(input);

        // Assert
        result.Should().NotBeNull();
        result.Xirr.Should().BeApproximately(0.12, 0.05);
    }

    [Fact]
    public void Compute_LeapYearSpan_HandlesCorrectly()
    {
        // Arrange - Span includes leap year 2020
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = new DateTime(2019, 1, 1),
            InvestmentMaturityDate = new DateTime(2021, 1, 1),
            MonthlyInvestmentAmount = 10000,
            ExpectedAnnualReturnRate = 12.0
        };

        // Act
        var result = CalculateXirr(input);

        // Assert
        result.Should().NotBeNull();
        result.Xirr.Should().BeApproximately(0.12, 0.05);
    }

    [Fact]
    public void Compute_MaximumValidInput_ReturnsCorrectResult()
    {
        // Arrange
        var input = new XirrInputViewModel
        {
            InvestmentStartDate = new DateTime(2020, 1, 1),
            InvestmentMaturityDate = new DateTime(2025, 1, 1),
            MonthlyInvestmentAmount = 10000000,
            ExpectedAnnualReturnRate = 100.0
        };

        // Act
        var result = CalculateXirr(input);

        // Assert
        result.Should().NotBeNull();
        result.Xirr.Should().BeGreaterThan(0.0);
    }

    [Theory]
    [InlineData("2020-01-01", "2025-01-01", 12.0, 15.0)]
    [InlineData("2020-01-01", "2025-01-01", 8.0, 10.0)]
    public void Compute_HigherExpectedReturn_ProducesHigherXirr(
        string startDate,
        string maturityDate,
        double lowerReturn,
        double higherReturn)
    {
        // Arrange
        var inputLower = new XirrInputViewModel
        {
            InvestmentStartDate = DateTime.Parse(startDate),
            InvestmentMaturityDate = DateTime.Parse(maturityDate),
            MonthlyInvestmentAmount = 10000,
            ExpectedAnnualReturnRate = lowerReturn
        };

        var inputHigher = new XirrInputViewModel
        {
            InvestmentStartDate = DateTime.Parse(startDate),
            InvestmentMaturityDate = DateTime.Parse(maturityDate),
            MonthlyInvestmentAmount = 10000,
            ExpectedAnnualReturnRate = higherReturn
        };

        // Act
        var resultLower = CalculateXirr(inputLower);
        var resultHigher = CalculateXirr(inputHigher);

        // Assert
        resultHigher.Xirr.Should().BeGreaterThan(resultLower.Xirr);
    }

    // Helper method that mimics the calculator logic
    private static XirrResultViewModel CalculateXirr(XirrInputViewModel input)
    {
        XirrCalculator calculator = new();
        return calculator.Compute(input);
    }

    // Helper to build cashflows for verification
    private static List<(DateTime Date, double Amount)> BuildTestCashflows(XirrInputViewModel input)
    {
        var cashflows = new List<(DateTime Date, double Amount)>();
        var startDate = input.InvestmentStartDate.Date;
        var maturityDate = input.InvestmentMaturityDate.Date;

        for (var date = startDate; date < maturityDate; date = date.AddMonths(1)) cashflows.Add((date, -input.MonthlyInvestmentAmount));

        var totalMonths = cashflows.Count;
        var monthlyRate = input.ExpectedAnnualReturnRate / (12d * 100d);
        var maturityAmount = monthlyRate == 0
            ? input.MonthlyInvestmentAmount * totalMonths
            : input.MonthlyInvestmentAmount
              * (Math.Pow(1 + monthlyRate, totalMonths) - 1)
              / monthlyRate
              * (1 + monthlyRate);

        cashflows.Add((maturityDate, maturityAmount));

        return cashflows;
    }

    // Helper to compute NPV for verification
    private static double ComputeNpv(List<(DateTime Date, double Amount)> cashflows, DateTime originDate, double rate)
    {
        var value = 0d;

        foreach (var (date, amount) in cashflows)
        {
            var years = (date - originDate).TotalDays / 365d;
            value += amount / Math.Pow(1 + rate, years);
        }

        return value;
    }
}