namespace FinSkew.Ui.Services.Validators.InputModels;

public class XirrInputViewModelValidator : AbstractValidator<XirrInputViewModel>
{
    public XirrInputViewModelValidator()
    {
        RuleFor(x => x.InvestmentStartDate)
            .Must(startDate => startDate.Date >= DateTime.Today.AddYears(-100).Date)
            .WithMessage("Investment start date is out of allowed range.")
            .Must((input, startDate) => startDate.Date < input.InvestmentMaturityDate.Date)
            .WithMessage("Investment start date must be before investment end date.");

        RuleFor(x => x.InvestmentMaturityDate)
            .Must((input, endDate) => endDate.Date > input.InvestmentStartDate.Date)
            .WithMessage("Investment end date must be after investment start date.")
            .Must(endDate => endDate.Date <= DateTime.Today.AddYears(100).Date)
            .WithMessage("Investment end date is out of allowed range.");

        RuleFor(x => x.MonthlyInvestmentAmount)
            .InclusiveBetween(500, 10000000)
            .WithMessage("Monthly investment amount must be between 500 and 10000000.");

        RuleFor(x => x.ExpectedAnnualReturnRate)
            .InclusiveBetween(0, 100)
            .WithMessage("Expected annual return rate must be between 0 and 100.");
    }
}