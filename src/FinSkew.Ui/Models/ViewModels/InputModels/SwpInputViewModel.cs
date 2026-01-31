namespace FinSkew.Ui.Models.ViewModels.InputModels;

public class SwpInputViewModel
{
    public int TotalInvestmentAmount { get; set; } = 500000;

    public int MonthlyWithdrawalAmount { get; set; } = 10000;

    public double ExpectedAnnualReturnRate { get; set; } = 8.0;

    public int TimePeriodInYears { get; set; } = 5;

    public CultureInfo Culture { get; } = CultureInfo.GetCultureInfo("en-In");
}