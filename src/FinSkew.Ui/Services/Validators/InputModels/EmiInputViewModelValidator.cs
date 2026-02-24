namespace FinSkew.Ui.Services.Validators.InputModels;

public class EmiInputViewModelValidator : AbstractValidator<EmiInputViewModel>
{
    public EmiInputViewModelValidator()
    {
        RuleFor(x => x.PrincipalAmount)
            .InclusiveBetween(10000, 100000000)
            .WithMessage("Loan amount must be between 10000 and 100000000.");

        RuleFor(x => x.AnnualInterestRate)
            .InclusiveBetween(0, 100)
            .WithMessage("Annual interest rate must be between 0 and 100.");

        RuleFor(x => x.LoanTenureInYears)
            .InclusiveBetween(1, 50)
            .WithMessage("Loan tenure in years must be between 1 and 50.");
    }
}