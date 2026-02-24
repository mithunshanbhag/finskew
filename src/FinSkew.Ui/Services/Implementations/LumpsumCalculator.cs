namespace FinSkew.Ui.Services.Implementations;

public class LumpsumCalculator(IValidator<LumpsumInputViewModel> validator) : CalculatorBase<LumpsumInputViewModel, LumpsumResultViewModel>(validator)
{
    public override LumpsumResultViewModel Compute(LumpsumInputViewModel input)
    {
        ValidateInput(input);

        var maturityAmount = (int)(input.PrincipalAmount * Math.Pow(1 + input.RateOfInterest / 100, input.TimePeriodInYears));
        var totalGain = maturityAmount - input.PrincipalAmount;
        var yearlyGrowth = new int[input.TimePeriodInYears];

        for (var year = 1; year <= input.TimePeriodInYears; year++) yearlyGrowth[year - 1] = (int)(input.PrincipalAmount * Math.Pow(1 + input.RateOfInterest / 100, year));

        return new LumpsumResultViewModel
        {
            Inputs = input,
            MaturityAmount = maturityAmount,
            TotalGain = totalGain,
            YearlyGrowth = yearlyGrowth
        };
    }
}