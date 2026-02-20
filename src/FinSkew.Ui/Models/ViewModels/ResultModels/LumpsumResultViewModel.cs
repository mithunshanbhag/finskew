namespace FinSkew.Ui.Models.ViewModels.ResultModels;

public class LumpsumResultViewModel
{
    #region Hidden fields

    public required LumpsumInputViewModel Inputs { get; init; }

    public required int TotalGain { get; init; }

    public required int MaturityAmount { get; init; }

    public required int[] YearlyGrowth { get; init; }

    #endregion

    #region Computed fields

    public string InvestedAmountStr => Inputs.PrincipalAmount.ToString("C0", Inputs.Culture);

    public string MaturityAmountStr => MaturityAmount.ToString("C0", Inputs.Culture);

    public string TotalGainStr => TotalGain.ToString("C0", Inputs.Culture);

    public string[] YearlyGrowthAsStr => YearlyGrowth.Select(amount => amount.ToString("C0", Inputs.Culture)).ToArray();

    #endregion
}
