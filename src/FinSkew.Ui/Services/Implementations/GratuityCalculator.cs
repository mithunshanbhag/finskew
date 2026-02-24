namespace FinSkew.Ui.Services.Implementations;

public class GratuityCalculator(IValidator<GratuityInputViewModel> validator) : CalculatorBase<GratuityInputViewModel, GratuityResultViewModel>(validator)
{
    public override GratuityResultViewModel Compute(GratuityInputViewModel input)
    {
        ValidateInput(input);

        var gratuityAmount = input.YearsOfService < 5
            ? 0
            : (long)(15d * input.Salary * input.YearsOfService / 26d);

        return new GratuityResultViewModel
        {
            Inputs = input,
            TotalSalaryDrawn = (long)input.Salary * 12 * input.YearsOfService,
            GratuityAmount = gratuityAmount
        };
    }
}