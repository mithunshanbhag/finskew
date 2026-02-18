namespace FinSkew.Ui.Models.ViewModels.ResultModels;

public class XirrResultViewModel
{
    #region Hidden fields

    public required XirrInputViewModel Inputs { get; init; }

    public required double InitialPrincipal { get; init; }

    public required double TotalGain { get; init; }

    public required double FinalAmount { get; init; }

    public required double Xirr { get; init; }

    #endregion

    #region Computed fields

    public string InitialPrincipalStr => InitialPrincipal.ToString("C0", Inputs.Culture);

    public string TotalGainStr => TotalGain.ToString("C0", Inputs.Culture);

    public string FinalAmountStr => FinalAmount.ToString("C0", Inputs.Culture);

    public string XirrStr => string.Format(Inputs.Culture, "{0:N2}%", Xirr * 100);

    #endregion
}