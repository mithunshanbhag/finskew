namespace FinSkew.Ui.Services.Implementations;

public class CompoundInterestCalculator : CalculatorBase<CompoundInterestInputViewModel, CompoundInterestResultViewModel>
{
    public override CompoundInterestResultViewModel Compute(CompoundInterestInputViewModel input)
    {
        var totalAmount = ComputeTotalAmount(input.PrincipalAmount, input.RateOfInterest, input.TimePeriodInYears,
            input.CompoundingFrequencyPerYear);
        var interestEarned = totalAmount - input.PrincipalAmount;
        var yearlyGrowth = new int[input.TimePeriodInYears];

        for (var year = 1; year <= input.TimePeriodInYears; year++)
            yearlyGrowth[year - 1] =
                ComputeTotalAmount(input.PrincipalAmount, input.RateOfInterest, year, input.CompoundingFrequencyPerYear);

        return new CompoundInterestResultViewModel
        {
            Inputs = input,
            TotalAmount = totalAmount,
            TotalInterestEarned = interestEarned,
            YearlyGrowth = yearlyGrowth
        };
    }

    private static int ComputeTotalAmount(int principal, double rateOfInterest, int years, int compoundingFrequencyPerYear)
    {
        return (int)(principal * Math.Pow(1 + rateOfInterest / 100 / compoundingFrequencyPerYear,
            years * compoundingFrequencyPerYear));
    }
}