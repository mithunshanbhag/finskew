namespace FinSkew.Ui.Models.ViewModels.InputModels;

public class EmiInputViewModel
{
    public int PrincipalAmount { get; set; } = 100000;

    public double AnnualInterestRate { get; set; } = 8.5;

    public int LoanTenureInYears { get; set; } = 20;

    public CultureInfo Culture { get; } = CultureInfo.GetCultureInfo("en-In");
}