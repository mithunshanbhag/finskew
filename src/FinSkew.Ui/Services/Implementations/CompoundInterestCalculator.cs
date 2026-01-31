namespace FinSkew.Ui.Services.Implementations;

public class CompoundInterestCalculator : CalculatorBase<CompoundInterestInputViewModel, CompoundInterestResultViewModel>
{
    public override CompoundInterestResultViewModel Compute(CompoundInterestInputViewModel input)
    {
        var rate = input.RateOfInterest / 100;
        var years = input.TimePeriodInYears;
        var frequency = input.CompoundingFrequencyPerYear;

        var totalAmount = (int)(input.PrincipalAmount * Math.Pow(1 + rate / frequency, years * frequency));
        var interestEarned = totalAmount - input.PrincipalAmount;

        return new CompoundInterestResultViewModel
        {
            Inputs = input,
            TotalAmount = totalAmount,
            TotalInterestEarned = interestEarned
        };
    }
}