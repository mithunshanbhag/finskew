namespace FinSkew.Ui.Services.Validators.InputModels;

public class SwpInputViewModelValidator : AbstractValidator<SwpInputViewModel>
{
    public SwpInputViewModelValidator()
    {
        RuleFor(x => x.TotalInvestmentAmount)
            .InclusiveBetween(10000, 100000000)
            .WithMessage("Invested amount must be between 10000 and 100000000.");

        RuleFor(x => x.MonthlyWithdrawalAmount)
            .InclusiveBetween(500, 10000000)
            .WithMessage("Monthly withdrawal amount must be between 500 and 10000000.");

        RuleFor(x => x.ExpectedAnnualReturnRate)
            .InclusiveBetween(0, 100)
            .WithMessage("Expected annual return rate must be between 0 and 100.");

        RuleFor(x => x.TimePeriodInYears)
            .InclusiveBetween(1, 50)
            .WithMessage("Time period in years must be between 1 and 50.");
    }
}