namespace FinSkew.Ui.Services.Implementations;

public class MisCalculator(IValidator<MisInputViewModel> validator)
    : CalculatorBase<MisInputViewModel, MisResultViewModel>(validator)
{
    public override MisResultViewModel Compute(MisInputViewModel input)
    {
        ValidateInput(input);

        var finalAmount = (int)(input.InvestedAmount *
            Math.Pow(1 + input.AnnualInterestRate / 100 / 4, 4 * input.TimePeriodInYears));
        var totalGain = finalAmount - input.InvestedAmount;

        return new MisResultViewModel
        {
            Inputs = input,
            TotalGain = totalGain,
            FinalAmount = finalAmount
        };
    }
}
