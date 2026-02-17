namespace FinSkew.Ui.Models.ViewModels.InputModels;

public class ScssInputViewModel
{
    public int PrincipalAmount { get; set; } = 10000;

    public double AnnualInterestRate { get; } = 7.4;

    public int TenureInYears { get; } = 5;

    public CultureInfo Culture { get; } = CultureInfo.GetCultureInfo("en-In");
}