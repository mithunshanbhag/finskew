namespace FinSkew.Ui.Models.ViewModels.ResultModels;

public class CagrResultViewModel
{
    #region Hidden fields

    public required CagrInputViewModel Inputs { get; init; }

    public required int TotalGain { get; init; }

    public required double CompoundAnnualGrowthRate { get; init; }

    public required int[] YearlyGrowth { get; init; }

    #endregion

    #region Computed fields

    public string InitialPrincipalAmountStr => Inputs.InitialPrincipalAmount.ToString("C0", Inputs.Culture);

    public string TotalGainStr => TotalGain.ToString("C0", Inputs.Culture);

    public string FinalAmountStr => Inputs.FinalAmount.ToString("C0", Inputs.Culture);

    public string CompoundAnnualGrowthRateStr => string.Format(Inputs.Culture, "{0:N2}%", CompoundAnnualGrowthRate);

    public string[] YearlyGrowthAsStr => YearlyGrowth.Select(amount => amount.ToString("C0", Inputs.Culture)).ToArray();

    #endregion
}