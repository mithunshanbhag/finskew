namespace FinSkew.Ui.Services.Validators.InputModels;

public class SipInputViewModelValidator : AbstractValidator<SipInputViewModel>
{
    public SipInputViewModelValidator()
    {
        RuleFor(x => x.MonthlyInvestment)
            .InclusiveBetween(500, 10000000)
            .WithMessage("Monthly investment must be between 500 and 10000000.");

        RuleFor(x => x.ExpectedReturnRate)
            .InclusiveBetween(1, 100)
            .WithMessage("Expected return rate must be between 1 and 100.");

        RuleFor(x => x.TimePeriodInYears)
            .InclusiveBetween(1, 50)
            .WithMessage("Time period in years must be between 1 and 50.");
    }
}