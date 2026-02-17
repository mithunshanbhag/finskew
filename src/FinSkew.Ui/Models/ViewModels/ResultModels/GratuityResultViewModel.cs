namespace FinSkew.Ui.Models.ViewModels.ResultModels;

public class GratuityResultViewModel
{
    public required GratuityInputViewModel Inputs { get; init; }

    public required long GratuityAmount { get; init; }

    public string GratuityAmountStr => GratuityAmount.ToString("C0", Inputs.Culture);
}