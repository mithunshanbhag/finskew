namespace FinSkew.Ui.Services.Implementations;

public class LumpsumCalculator : CalculatorBase<LumpsumInputViewModel, LumpsumResultViewModel>
{
    public override LumpsumResultViewModel Compute(LumpsumInputViewModel input)
    {
        var maturityAmount = (int)(input.PrincipalAmount * Math.Pow(1 + input.RateOfInterest / 100, input.TimePeriodInYears));
        var totalGain = maturityAmount - input.PrincipalAmount;

        return new LumpsumResultViewModel
        {
            Inputs = input,
            MaturityAmount = maturityAmount,
            TotalGain = totalGain
        };
    }
}