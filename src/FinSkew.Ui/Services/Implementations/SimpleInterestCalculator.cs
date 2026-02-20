namespace FinSkew.Ui.Services.Implementations;

public class SimpleInterestCalculator : CalculatorBase<SimpleInterestInputViewModel, SimpleInterestResultViewModel>
{
    public override SimpleInterestResultViewModel Compute(SimpleInterestInputViewModel input)
    {
        #region Compute results

        var totalAmount = ComputeTotalAmount(input.PrincipalAmount, input.RateOfInterest, input.TimePeriodInYears);
        var interestEarned = totalAmount - input.PrincipalAmount;

        #endregion

        #region Compute yearly growth

        var yearlyGrowth = new int[input.TimePeriodInYears];
        for (var year = 1; year <= input.TimePeriodInYears; year++) yearlyGrowth[year - 1] = ComputeTotalAmount(input.PrincipalAmount, input.RateOfInterest, year);

        #endregion

        return new SimpleInterestResultViewModel
        {
            Inputs = input,
            TotalInterestEarned = interestEarned,
            TotalAmount = totalAmount,
            YearlyGrowth = yearlyGrowth
        };
    }

    private static int ComputeTotalAmount(int principal, double rate, int years)
    {
        return (int)(principal * (1 + rate / 100 * years));
    }
}