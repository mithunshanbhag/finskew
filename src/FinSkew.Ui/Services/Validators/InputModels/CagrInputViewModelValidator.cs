namespace FinSkew.Ui.Services.Validators.InputModels;

public class CagrInputViewModelValidator : AbstractValidator<CagrInputViewModel>
{
    public CagrInputViewModelValidator()
    {
        RuleFor(x => x.InitialPrincipalAmount)
            .InclusiveBetween(10000, 100000000)
            .WithMessage("Invested amount must be between 10000 and 100000000.");

        RuleFor(x => x.FinalAmount)
            .InclusiveBetween(10000, 100000000)
            .WithMessage("Final amount must be between 10000 and 100000000.");

        RuleFor(x => x.TimePeriodInYears)
            .InclusiveBetween(1, 100)
            .WithMessage("Time period in years must be between 1 and 100.");
    }
}