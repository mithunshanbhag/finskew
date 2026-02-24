namespace FinSkew.Ui.Services.Validators.InputModels;

public class LumpsumInputViewModelValidator : AbstractValidator<LumpsumInputViewModel>
{
    public LumpsumInputViewModelValidator()
    {
        RuleFor(x => x.PrincipalAmount)
            .InclusiveBetween(10000, 100000000)
            .WithMessage("Principal amount must be between 10000 and 100000000.");

        RuleFor(x => x.RateOfInterest)
            .InclusiveBetween(1, 100)
            .WithMessage("Rate of interest must be between 1 and 100.");

        RuleFor(x => x.TimePeriodInYears)
            .InclusiveBetween(1, 100)
            .WithMessage("Time period in years must be between 1 and 100.");
    }
}