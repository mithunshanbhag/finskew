namespace FinSkew.Ui.Models.ViewModels.ResultModels;

public class ScssResultViewModel
{
    public required ScssInputViewModel Inputs { get; init; }

    public required int TotalInterestEarned { get; init; }

    public required int MaturityAmount { get; init; }

    public string PrincipalAmountStr => Inputs.PrincipalAmount.ToString("C0", Inputs.Culture);

    public string TotalInterestEarnedStr => TotalInterestEarned.ToString("C0", Inputs.Culture);

    public string MaturityAmountStr => MaturityAmount.ToString("C0", Inputs.Culture);
}