namespace FinSkew.Ui.Models.ViewModels.ResultModels;

public class SimpleInterestResultViewModel
{
    #region Hidden fields

    public required SimpleInterestInputViewModel Inputs { get; init; }

    public required int TotalInterestEarned { get; init; }

    public required int TotalAmount { get; set; }

    public required int[] YearlyGrowth { get; set; }

    #endregion

    #region Computed fields

    public string PrincipalAmountStr => Inputs.PrincipalAmount.ToString("C0", Inputs.Culture);

    public string TotalAmountStr => TotalAmount.ToString("C0", Inputs.Culture);

    public string TotalInterestEarnedStr => TotalInterestEarned.ToString("C0", Inputs.Culture);

    public string[] YearlyGrowthAsStr => YearlyGrowth.Select(amount => amount.ToString("C0", Inputs.Culture)).ToArray();

    #endregion
}