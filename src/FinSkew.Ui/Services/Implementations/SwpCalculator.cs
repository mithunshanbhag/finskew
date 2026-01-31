namespace FinSkew.Ui.Services.Implementations;

public class SwpCalculator : CalculatorBase<SwpInputViewModel, SwpResultViewModel>
{
    public override SwpResultViewModel Compute(SwpInputViewModel input)
    {
        var monthlyRate = input.ExpectedAnnualReturnRate / (12 * 100);
        var totalMonths = input.TimePeriodInYears * 12;
        var totalWithdrawal = input.MonthlyWithdrawalAmount * totalMonths;
        var growthFactor = Math.Pow(1 + monthlyRate, totalMonths);
        var totalMaturityAmount = monthlyRate == 0
            ? input.TotalInvestmentAmount - totalWithdrawal
            : input.TotalInvestmentAmount * growthFactor
              - input.MonthlyWithdrawalAmount * ((growthFactor - 1) / monthlyRate) * (1 + monthlyRate);

        return new SwpResultViewModel
        {
            Inputs = input,
            TotalWithdrawal = totalWithdrawal,
            TotalMaturityAmount = (int)Math.Round(totalMaturityAmount, MidpointRounding.AwayFromZero)
        };
    }
}