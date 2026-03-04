namespace FinSkew.Ui.Services.Validators.InputModels;

public class RecurringDepositInputViewModelValidator : AbstractValidator<RecurringDepositInputViewModel>
{
    public RecurringDepositInputViewModelValidator()
    {
        RuleFor(x => x.MonthlyDepositAmount)
            .InclusiveBetween(1000, 1000000)
            .WithMessage("Monthly deposit amount must be between 1000 and 1000000.");

        RuleFor(x => x.ExpectedAnnualInterestRate)
            .InclusiveBetween(0, 100)
            .WithMessage("Expected annual interest rate must be between 0 and 100.");

        RuleFor(x => x.TimePeriodInYears)
            .InclusiveBetween(1, 50)
            .WithMessage("Time period in years must be between 1 and 50.");
    }
}