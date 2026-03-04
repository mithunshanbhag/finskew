namespace FinSkew.Ui.Models.ViewModels.InputModels;

public class RecurringDepositInputViewModel
{
    public int MonthlyDepositAmount { get; set; } = 5000;

    public double ExpectedAnnualInterestRate { get; set; } = 6.0;

    public int TimePeriodInYears { get; set; } = 5;

    public CultureInfo Culture { get; } = CultureInfo.GetCultureInfo("en-In");
}