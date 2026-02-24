namespace FinSkew.Ui.Services.Validators.InputModels;

public class ScssInputViewModelValidator : AbstractValidator<ScssInputViewModel>
{
    public ScssInputViewModelValidator()
    {
        RuleFor(x => x.PrincipalAmount)
            .InclusiveBetween(10000, 100000000)
            .WithMessage("Principal amount must be between 10000 and 100000000.");

        RuleFor(x => x.AnnualInterestRate)
            .Equal(7.4)
            .WithMessage("Annual interest rate must be 7.4.");

        RuleFor(x => x.TenureInYears)
            .Equal(5)
            .WithMessage("Tenure in years must be 5.");
    }
}