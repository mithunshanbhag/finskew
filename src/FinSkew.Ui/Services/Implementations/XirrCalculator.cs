namespace FinSkew.Ui.Services.Implementations;

public class XirrCalculator(IValidator<XirrInputViewModel> validator) : CalculatorBase<XirrInputViewModel, XirrResultViewModel>(validator)
{
    private const double MinRate = -0.999999999d;
    private const double Tolerance = 1e-10;
    private const double DerivativeTolerance = 1e-12;
    private const int MaxNewtonIterations = 50;
    private const int MaxBisectionIterations = 200;
    private const int MaxBracketingIterations = 60;

    public override XirrResultViewModel Compute(XirrInputViewModel input)
    {
        ValidateInput(input);

        var startDate = input.InvestmentStartDate.Date;
        var maturityDate = input.InvestmentMaturityDate.Date;

        var calculation = BuildCashflows(input, startDate, maturityDate);
        var initialGuess = input.ExpectedAnnualReturnRate / 100d;
        var xirr = Solve(calculation.Cashflows, startDate, initialGuess);
        var totalMonths = calculation.Cashflows.Count - 1;
        var yearlyGrowth = BuildYearlyGrowth(input.MonthlyInvestmentAmount, input.ExpectedAnnualReturnRate, totalMonths);

        return new XirrResultViewModel
        {
            Inputs = input,
            InitialPrincipal = calculation.InitialPrincipal,
            TotalGain = calculation.TotalGain,
            FinalAmount = calculation.FinalAmount,
            Xirr = xirr,
            YearlyGrowth = yearlyGrowth
        };
    }

    private static (List<(DateTime Date, double Amount)> Cashflows, double InitialPrincipal, double TotalGain, double FinalAmount) BuildCashflows(
        XirrInputViewModel input,
        DateTime startDate,
        DateTime maturityDate)
    {
        var cashflows = new List<(DateTime Date, double Amount)>();

        for (var date = startDate; date < maturityDate; date = date.AddMonths(1)) cashflows.Add((date, -input.MonthlyInvestmentAmount));

        var totalMonths = cashflows.Count;
        var initialPrincipal = input.MonthlyInvestmentAmount * totalMonths;
        var monthlyRate = input.ExpectedAnnualReturnRate / (12d * 100d);
        var finalAmount = ComputeFinalAmount(input.MonthlyInvestmentAmount, monthlyRate, totalMonths);
        var totalGain = finalAmount - initialPrincipal;

        cashflows.Add((maturityDate, finalAmount));

        return (cashflows, initialPrincipal, totalGain, finalAmount);
    }

    private static int[] BuildYearlyGrowth(int monthlyInvestmentAmount, double annualReturnRate, int totalMonths)
    {
        if (totalMonths <= 0) return [];

        var monthlyRate = annualReturnRate / (12d * 100d);
        var yearlyPoints = (int)Math.Ceiling(totalMonths / 12d);
        var yearlyGrowth = new int[yearlyPoints];

        for (var year = 1; year <= yearlyPoints; year++)
        {
            var months = Math.Min(year * 12, totalMonths);
            var endOfYearAmount = ComputeFinalAmount(monthlyInvestmentAmount, monthlyRate, months);
            yearlyGrowth[year - 1] = (int)Math.Round(endOfYearAmount, MidpointRounding.AwayFromZero);
        }

        return yearlyGrowth;
    }

    private static double ComputeFinalAmount(int monthlyInvestmentAmount, double monthlyRate, int totalMonths)
    {
        return monthlyRate == 0
            ? monthlyInvestmentAmount * totalMonths
            : monthlyInvestmentAmount
              * (Math.Pow(1 + monthlyRate, totalMonths) - 1)
              / monthlyRate
              * (1 + monthlyRate);
    }

    private static double Solve(List<(DateTime Date, double Amount)> cashflows, DateTime originDate, double initialGuess)
    {
        var guess = Math.Clamp(initialGuess, -0.9d, 1d);
        var newtonRate = guess;

        for (var iteration = 0; iteration < MaxNewtonIterations; iteration++)
        {
            var npv = ComputeNpv(cashflows, originDate, newtonRate);
            if (Math.Abs(npv) <= Tolerance) return newtonRate;

            var derivative = ComputeNpvDerivative(cashflows, originDate, newtonRate);
            if (Math.Abs(derivative) <= DerivativeTolerance) break;

            var nextRate = newtonRate - npv / derivative;
            if (double.IsNaN(nextRate) || double.IsInfinity(nextRate) || nextRate <= MinRate) break;

            if (Math.Abs(nextRate - newtonRate) <= Tolerance) return nextRate;

            newtonRate = nextRate;
        }

        var lowerBound = MinRate;
        var upperBound = Math.Max(guess, 0.1d);
        var lowerValue = ComputeNpv(cashflows, originDate, lowerBound);
        var upperValue = ComputeNpv(cashflows, originDate, upperBound);

        for (var iteration = 0; iteration < MaxBracketingIterations && lowerValue * upperValue > 0; iteration++)
        {
            upperBound = upperBound * 2d + 0.1d;
            upperValue = ComputeNpv(cashflows, originDate, upperBound);
        }

        if (lowerValue * upperValue > 0) return newtonRate;

        for (var iteration = 0; iteration < MaxBisectionIterations; iteration++)
        {
            var midpoint = (lowerBound + upperBound) / 2d;
            var midpointValue = ComputeNpv(cashflows, originDate, midpoint);

            if (Math.Abs(midpointValue) <= Tolerance || Math.Abs(upperBound - lowerBound) <= Tolerance) return midpoint;

            if (lowerValue * midpointValue < 0)
            {
                upperBound = midpoint;
                upperValue = midpointValue;
                continue;
            }

            lowerBound = midpoint;
            lowerValue = midpointValue;
        }

        return (lowerBound + upperBound) / 2d;
    }

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

    private static double ComputeNpvDerivative(List<(DateTime Date, double Amount)> cashflows, DateTime originDate, double rate)
    {
        var derivative = 0d;

        foreach (var (date, amount) in cashflows)
        {
            var years = (date - originDate).TotalDays / 365d;
            derivative += -years * amount / Math.Pow(1 + rate, years + 1);
        }

        return derivative;
    }
}