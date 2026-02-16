namespace FinSkew.Ui.Models.ViewModels.InputModels;

public class XirrInputViewModel
{
    public DateTime InvestmentStartDate { get; set; } = DateTime.Today.AddYears(-5);

    public DateTime InvestmentMaturityDate { get; set; } = DateTime.Today;

    public int MonthlyInvestmentAmount { get; set; } = 1000;

    public double ExpectedAnnualReturnRate { get; set; } = 12.0;

    public CultureInfo Culture { get; } = CultureInfo.GetCultureInfo("en-In");
}