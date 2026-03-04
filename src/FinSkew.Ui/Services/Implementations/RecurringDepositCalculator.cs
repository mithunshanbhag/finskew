namespace FinSkew.Ui.Services.Implementations;

public class RecurringDepositCalculator(IValidator<RecurringDepositInputViewModel> validator)
    : CalculatorBase<RecurringDepositInputViewModel, RecurringDepositResultViewModel>(validator)
{
    public override RecurringDepositResultViewModel Compute(RecurringDepositInputViewModel input)
    {
        ValidateInput(input);

        var totalMonths = input.TimePeriodInYears * 12;
        var monthlyRate = input.ExpectedAnnualInterestRate / (12 * 100);
        var totalInvested = input.MonthlyDepositAmount * totalMonths;
        var maturityAmount = monthlyRate == 0
            ? input.MonthlyDepositAmount * totalMonths
            : (int)(input.MonthlyDepositAmount *
                    ((Math.Pow(1 + monthlyRate, totalMonths) - 1) / monthlyRate));
        var totalGain = maturityAmount - totalInvested;

        return new RecurringDepositResultViewModel
        {
            Inputs = input,
            TotalInvested = totalInvested,
            TotalGain = totalGain,
            MaturityAmount = maturityAmount
        };
    }
}