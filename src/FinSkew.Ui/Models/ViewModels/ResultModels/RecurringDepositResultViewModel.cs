namespace FinSkew.Ui.Models.ViewModels.ResultModels;

public class RecurringDepositResultViewModel
{
    public required RecurringDepositInputViewModel Inputs { get; init; }

    public required int TotalInvested { get; init; }

    public required int TotalGain { get; init; }

    public required int MaturityAmount { get; init; }

    public int InvestedAmount => TotalInvested;

    public int FinalAmount => MaturityAmount;

    public string TotalInvestedStr => TotalInvested.ToString("C0", Inputs.Culture);

    public string TotalGainStr => TotalGain.ToString("C0", Inputs.Culture);

    public string MaturityAmountStr => MaturityAmount.ToString("C0", Inputs.Culture);

    public string InvestedAmountStr => TotalInvestedStr;

    public string FinalAmountStr => MaturityAmountStr;
}