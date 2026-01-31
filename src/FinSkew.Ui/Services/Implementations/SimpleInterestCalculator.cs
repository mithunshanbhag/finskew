namespace FinSkew.Ui.Services.Implementations;

public class SimpleInterestCalculator : CalculatorBase<SimpleInterestInputViewModel, SimpleInterestResultViewModel>
{
    public override SimpleInterestResultViewModel Compute(SimpleInterestInputViewModel input)
    {
        var totalAmount = (int)(input.PrincipalAmount * (1 + input.RateOfInterest / 100 * input.TimePeriodInYears));
        var interestEarned = totalAmount - input.PrincipalAmount;

        return new SimpleInterestResultViewModel
        {
            Inputs = input,
            TotalInterestEarned = interestEarned,
            TotalAmount = totalAmount
        };
    }
}