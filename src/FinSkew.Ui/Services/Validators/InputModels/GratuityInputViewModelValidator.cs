namespace FinSkew.Ui.Services.Validators.InputModels;

public class GratuityInputViewModelValidator : AbstractValidator<GratuityInputViewModel>
{
    public GratuityInputViewModelValidator()
    {
        RuleFor(x => x.Salary)
            .InclusiveBetween(10000, 100000000)
            .WithMessage("Salary must be between 10000 and 100000000.");

        RuleFor(x => x.YearsOfService)
            .InclusiveBetween(5, 50)
            .WithMessage("Years of service must be between 5 and 50.");
    }
}