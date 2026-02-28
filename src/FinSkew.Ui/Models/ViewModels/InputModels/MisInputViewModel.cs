namespace FinSkew.Ui.Models.ViewModels.InputModels;

public class MisInputViewModel
{
    public int InvestedAmount { get; set; } = 100000;

    public double AnnualInterestRate { get; set; } = 6.6;

    public int TimePeriodInYears { get; } = 5;

    public CultureInfo Culture { get; } = CultureInfo.GetCultureInfo("en-In");
}
