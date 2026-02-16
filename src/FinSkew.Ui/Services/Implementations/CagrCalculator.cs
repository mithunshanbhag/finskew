namespace FinSkew.Ui.Services.Implementations;

public class CagrCalculator : CalculatorBase<CagrInputViewModel, CagrResultViewModel>
{
    public override CagrResultViewModel Compute(CagrInputViewModel input)
    {
        var compoundAnnualGrowthRate =
            (Math.Pow((double)input.FinalAmount / input.InitialPrincipalAmount, 1d / input.TimePeriodInYears) - 1) * 100;

        return new CagrResultViewModel
        {
            Inputs = input,
            CompoundAnnualGrowthRate = compoundAnnualGrowthRate
        };
    }
}