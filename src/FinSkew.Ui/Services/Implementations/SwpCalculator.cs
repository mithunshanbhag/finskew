namespace FinSkew.Ui.Services.Implementations;

public class SwpCalculator : CalculatorBase<SwpInputViewModel, SwpResultViewModel>
{
    public override SwpResultViewModel Compute(SwpInputViewModel input)
    {
        var monthlyRate = input.ExpectedAnnualReturnRate / (12 * 100);
        var totalMonths = input.TimePeriodInYears * 12;
        var totalWithdrawal = input.MonthlyWithdrawalAmount * totalMonths;
        var yearlyGrowth = new int[input.TimePeriodInYears];
        for (var year = 1; year <= input.TimePeriodInYears; year++)
            yearlyGrowth[year - 1] = ComputeMaturityAmount(input.TotalInvestmentAmount, input.MonthlyWithdrawalAmount,
                monthlyRate, year * 12);

        var totalMaturityAmount = ComputeMaturityAmount(input.TotalInvestmentAmount, input.MonthlyWithdrawalAmount,
            monthlyRate, totalMonths);

        return new SwpResultViewModel
        {
            Inputs = input,
            TotalWithdrawal = totalWithdrawal,
            TotalMaturityAmount = totalMaturityAmount,
            YearlyGrowth = yearlyGrowth
        };
    }

    private static int ComputeMaturityAmount(int totalInvestmentAmount, int monthlyWithdrawalAmount, double monthlyRate,
        int totalMonths)
    {
        if (monthlyRate == 0) return totalInvestmentAmount - monthlyWithdrawalAmount * totalMonths;

        var growthFactor = Math.Pow(1 + monthlyRate, totalMonths);
        var maturityAmount = totalInvestmentAmount * growthFactor
                             - monthlyWithdrawalAmount * ((growthFactor - 1) / monthlyRate) * (1 + monthlyRate);
        return (int)Math.Round(maturityAmount, MidpointRounding.AwayFromZero);
    }
}