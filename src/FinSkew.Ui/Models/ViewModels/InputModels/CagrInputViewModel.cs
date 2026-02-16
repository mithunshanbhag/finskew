namespace FinSkew.Ui.Models.ViewModels.InputModels;

public class CagrInputViewModel
{
    public int InitialPrincipalAmount { get; set; } = 10000;

    public int FinalAmount { get; set; } = 12000;

    public int TimePeriodInYears { get; set; } = 3;

    public CultureInfo Culture { get; } = CultureInfo.GetCultureInfo("en-In");
}