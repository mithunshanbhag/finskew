namespace FinSkew.Ui.Services.Implementations;

public class StepUpSipCalculator : CalculatorBase<StepUpSipInputViewModel, StepUpSipResultViewModel>
{
    public override StepUpSipResultViewModel Compute(StepUpSipInputViewModel input)
    {
        #region Compute results

        var (totalInvestedInt, maturityAmountInt) = ComputeTotalInvestedAndMaturityAmount(input, input.TimePeriodInYears);
        var totalGain = maturityAmountInt - totalInvestedInt;

        #endregion

        #region Compute yearly growth

        var yearlyGrowth = new int[input.TimePeriodInYears];
        for (var year = 1; year <= input.TimePeriodInYears; year++)
        {
            yearlyGrowth[year - 1] = ComputeTotalInvestedAndMaturityAmount(input, year).MaturityAmount;
        }

        #endregion

        return new StepUpSipResultViewModel
        {
            Inputs = input,
            TotalInvested = totalInvestedInt,
            MaturityAmount = maturityAmountInt,
            TotalGain = totalGain,
            YearlyGrowth = yearlyGrowth
        };
    }

    private static (int TotalInvested, int MaturityAmount) ComputeTotalInvestedAndMaturityAmount(
        StepUpSipInputViewModel input,
        int years)
    {
        var monthlyRate = input.ExpectedReturnRate / (12 * 100);
        var annualStepUpRate = input.StepUpPercentage / 100;

        double maturityAmount = 0;
        double totalInvested = 0;

        for (var year = 0; year < years; year++)
        {
            var monthlySipForYear = input.MonthlyInvestment * Math.Pow(1 + annualStepUpRate, year);
            var futureValueOfYearSip = monthlySipForYear *
                                       ((Math.Pow(1 + monthlyRate, 12) - 1) / monthlyRate) *
                                       (1 + monthlyRate) *
                                       Math.Pow(1 + monthlyRate, 12 * (years - 1 - year));

            maturityAmount += futureValueOfYearSip;
            totalInvested += 12 * monthlySipForYear;
        }

        return ((int)totalInvested, (int)maturityAmount);
    }
}
