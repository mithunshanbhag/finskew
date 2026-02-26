namespace FinSkew.Ui.Services.Validators.InputModels;

public class MisInputViewModelValidator : AbstractValidator<MisInputViewModel>
{
    public MisInputViewModelValidator()
    {
        RuleFor(x => x.InvestedAmount)
            .InclusiveBetween(10000, 100000000)
            .WithMessage("Invested amount must be between 10000 and 100000000.");

        RuleFor(x => x.AnnualInterestRate)
            .InclusiveBetween(1, 100)
            .WithMessage("Annual interest rate must be between 1 and 100.");

        RuleFor(x => x.TimePeriodInYears)
            .Equal(5)
            .WithMessage("Time period in years must be 5.");
    }
}
