namespace FinSkew.Ui.Services.Validators.InputModels;

public class CompoundInterestInputViewModelValidator : AbstractValidator<CompoundInterestInputViewModel>
{
    public CompoundInterestInputViewModelValidator()
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

        RuleFor(x => x.CompoundingFrequencyPerYear)
            .InclusiveBetween(1, 365)
            .WithMessage("Compounding frequency must be between 1 and 365.");
    }
}