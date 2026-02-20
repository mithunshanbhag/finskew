namespace FinSkew.Ui.Services.Implementations;

public class CagrCalculator : CalculatorBase<CagrInputViewModel, CagrResultViewModel>
{
    public override CagrResultViewModel Compute(CagrInputViewModel input)
    {
        var totalGain = input.FinalAmount - input.InitialPrincipalAmount;
        var compoundAnnualGrowthRate =
            (Math.Pow((double)input.FinalAmount / input.InitialPrincipalAmount, 1d / input.TimePeriodInYears) - 1) * 100;
        var yearlyGrowth = new int[input.TimePeriodInYears];

        for (var year = 1; year <= input.TimePeriodInYears; year++) yearlyGrowth[year - 1] = ComputeYearEndAmount(input.InitialPrincipalAmount, compoundAnnualGrowthRate, year);

        yearlyGrowth[^1] = input.FinalAmount;

        return new CagrResultViewModel
        {
            Inputs = input,
            TotalGain = totalGain,
            CompoundAnnualGrowthRate = compoundAnnualGrowthRate,
            YearlyGrowth = yearlyGrowth
        };
    }

    private static int ComputeYearEndAmount(int initialPrincipalAmount, double compoundAnnualGrowthRate, int years)
    {
        return (int)(initialPrincipalAmount * Math.Pow(1 + compoundAnnualGrowthRate / 100, years));
    }
}