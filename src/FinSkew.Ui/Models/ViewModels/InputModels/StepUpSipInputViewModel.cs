namespace FinSkew.Ui.Models.ViewModels.InputModels;

public class StepUpSipInputViewModel
{
    public int MonthlyInvestment { get; set; } = 1000;

    public double StepUpPercentage { get; set; } = 5.0;

    public double ExpectedReturnRate { get; set; } = 12.0;

    public int TimePeriodInYears { get; set; } = 5;

    public CultureInfo Culture { get; } = CultureInfo.GetCultureInfo("en-In");
}