namespace FinSkew.Ui.Models.ViewModels.InputModels;

public class LumpsumInputViewModel
{
    public int PrincipalAmount { get; set; } = 10000;

    public double RateOfInterest { get; set; } = 5.0;

    public int TimePeriodInYears { get; set; } = 3;

    public CultureInfo Culture { get; } = CultureInfo.GetCultureInfo("en-In");
}