namespace FinSkew.Ui.Services.Implementations;

public class ScssCalculator(IValidator<ScssInputViewModel> validator) : CalculatorBase<ScssInputViewModel, ScssResultViewModel>(validator)
{
    public override ScssResultViewModel Compute(ScssInputViewModel input)
    {
        ValidateInput(input);

        var maturityAmount = (int)(input.PrincipalAmount * Math.Pow(1 + input.AnnualInterestRate / 100, input.TenureInYears));
        var totalInterestEarned = maturityAmount - input.PrincipalAmount;

        return new ScssResultViewModel
        {
            Inputs = input,
            TotalInterestEarned = totalInterestEarned,
            MaturityAmount = maturityAmount
        };
    }
}