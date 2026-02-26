namespace FinSkew.Ui.Models.ViewModels.ResultModels;

public class MisResultViewModel
{
    public required MisInputViewModel Inputs { get; init; }

    public required int TotalGain { get; init; }

    public required int MonthlyIncome { get; init; }

    public required int FinalAmount { get; init; }

    public string InvestedAmountStr => Inputs.InvestedAmount.ToString("C0", Inputs.Culture);

    public string TotalGainStr => TotalGain.ToString("C0", Inputs.Culture);

    public string MonthlyIncomeStr => MonthlyIncome.ToString("C0", Inputs.Culture);

    public string FinalAmountStr => FinalAmount.ToString("C0", Inputs.Culture);
}
