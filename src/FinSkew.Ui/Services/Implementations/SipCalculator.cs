namespace FinSkew.Ui.Services.Implementations;

public class SipCalculator : CalculatorBase<SipInputViewModel, SipResultViewModel>
{
    public override SipResultViewModel Compute(SipInputViewModel input)
    {
        var monthlyRate = input.ExpectedReturnRate / (12 * 100);
        var totalMonths = input.TimePeriodInYears * 12;
        var totalInvested = input.MonthlyInvestment * totalMonths;

        var maturityAmount = monthlyRate == 0
            ? totalInvested
            : (int)(input.MonthlyInvestment *
                    (Math.Pow(1 + monthlyRate, totalMonths) - 1) /
                    monthlyRate *
                    (1 + monthlyRate));
        var totalGain = maturityAmount - totalInvested;

        return new SipResultViewModel
        {
            Inputs = input,
            TotalInvested = totalInvested,
            MaturityAmount = maturityAmount,
            TotalGain = totalGain
        };
    }
}