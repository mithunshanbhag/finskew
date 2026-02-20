namespace FinSkew.Ui.Models.ViewModels.ResultModels;

public class SwpResultViewModel
{
    #region Hidden fields

    public required SwpInputViewModel Inputs { get; init; }

    public required int TotalWithdrawal { get; init; }

    public required int TotalMaturityAmount { get; init; }

    public required int[] YearlyGrowth { get; init; }

    #endregion

    #region Computed fields

    public string TotalInvestmentAmountStr => Inputs.TotalInvestmentAmount.ToString("C0", Inputs.Culture);

    public string TotalWithdrawalStr => TotalWithdrawal.ToString("C0", Inputs.Culture);

    public string TotalMaturityAmountStr => TotalMaturityAmount.ToString("C0", Inputs.Culture);

    public string[] YearlyGrowthAsStr => YearlyGrowth.Select(amount => amount.ToString("C0", Inputs.Culture)).ToArray();

    #endregion
}
