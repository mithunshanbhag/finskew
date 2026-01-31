namespace FinSkew.Ui.Models.ViewModels.ResultModels;

public class StepUpSipResultViewModel
{
    #region Hidden fields

    public required StepUpSipInputViewModel Inputs { get; init; }

    public required int TotalInvested { get; init; }

    public required int TotalGain { get; init; }

    public required int MaturityAmount { get; init; }

    #endregion

    #region Computed fields

    public string TotalInvestedStr => TotalInvested.ToString("C0", Inputs.Culture);

    public string MaturityAmountStr => MaturityAmount.ToString("C0", Inputs.Culture);

    public string TotalGainStr => TotalGain.ToString("C0", Inputs.Culture);

    #endregion
}