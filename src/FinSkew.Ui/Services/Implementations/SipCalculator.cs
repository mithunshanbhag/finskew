namespace FinSkew.Ui.Services.Implementations;

public class SipCalculator(IValidator<SipInputViewModel> validator) : CalculatorBase<SipInputViewModel, SipResultViewModel>(validator)
{
    public override SipResultViewModel Compute(SipInputViewModel input)
    {
        ValidateInput(input);

        var monthlyRate = input.ExpectedReturnRate / (12 * 100);
        var totalMonths = input.TimePeriodInYears * 12;
        var totalInvested = input.MonthlyInvestment * totalMonths;
        var yearlyGrowth = new int[input.TimePeriodInYears];
        for (var year = 1; year <= input.TimePeriodInYears; year++) yearlyGrowth[year - 1] = ComputeMaturityAmount(input.MonthlyInvestment, monthlyRate, year * 12);

        var maturityAmount = ComputeMaturityAmount(input.MonthlyInvestment, monthlyRate, totalMonths);
        var totalGain = maturityAmount - totalInvested;

        return new SipResultViewModel
        {
            Inputs = input,
            TotalInvested = totalInvested,
            MaturityAmount = maturityAmount,
            TotalGain = totalGain,
            YearlyGrowth = yearlyGrowth
        };
    }

    private static int ComputeMaturityAmount(int monthlyInvestment, double monthlyRate, int totalMonths)
    {
        return monthlyRate == 0
            ? monthlyInvestment * totalMonths
            : (int)(monthlyInvestment *
                    (Math.Pow(1 + monthlyRate, totalMonths) - 1) /
                    monthlyRate *
                    (1 + monthlyRate));
    }
}