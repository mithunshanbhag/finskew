namespace FinSkew.Ui.Services.Implementations;

public class StepUpSipCalculator : CalculatorBase<StepUpSipInputViewModel, StepUpSipResultViewModel>
{
    public override StepUpSipResultViewModel Compute(StepUpSipInputViewModel input)
    {
        var monthlyRate = input.ExpectedReturnRate / (12 * 100);
        var annualStepUpRate = input.StepUpPercentage / 100;

        double maturityAmount = 0;
        double totalInvested = 0;

        for (var year = 0; year < input.TimePeriodInYears; year++)
        {
            var monthlySipForYear = input.MonthlyInvestment * Math.Pow(1 + annualStepUpRate, year);

            var futureValueOfYearSip = monthlySipForYear *
                                       ((Math.Pow(1 + monthlyRate, 12) - 1) / monthlyRate) *
                                       (1 + monthlyRate) *
                                       Math.Pow(1 + monthlyRate, 12 * (input.TimePeriodInYears - 1 - year));

            maturityAmount += futureValueOfYearSip;
            totalInvested += 12 * monthlySipForYear;
        }

        var maturityAmountInt = (int)maturityAmount;
        var totalInvestedInt = (int)totalInvested;
        var totalGain = maturityAmountInt - totalInvestedInt;

        return new StepUpSipResultViewModel
        {
            Inputs = input,
            TotalInvested = totalInvestedInt,
            MaturityAmount = maturityAmountInt,
            TotalGain = totalGain
        };
    }
}