namespace FinSkew.Ui.Models.ViewModels.InputModels;

public class SipInputViewModel
{
    public int MonthlyInvestment { get; set; } = 1000;

    public double ExpectedReturnRate { get; set; } = 12.0;

    public int TimePeriodInYears { get; set; } = 5;

    public CultureInfo Culture { get; } = CultureInfo.GetCultureInfo("en-In");
}