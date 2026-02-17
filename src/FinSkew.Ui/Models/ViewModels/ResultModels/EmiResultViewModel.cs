namespace FinSkew.Ui.Models.ViewModels.ResultModels;

public class EmiResultViewModel
{
    public required EmiInputViewModel Inputs { get; init; }

    public required int MonthlyEmi { get; init; }

    public required int TotalPayment { get; init; }

    public required int TotalInterest { get; init; }

    public string MonthlyEmiStr => MonthlyEmi.ToString("C0", Inputs.Culture);

    public string TotalPaymentStr => TotalPayment.ToString("C0", Inputs.Culture);

    public string TotalInterestStr => TotalInterest.ToString("C0", Inputs.Culture);
}