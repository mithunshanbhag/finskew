namespace FinSkew.Ui.Models.ViewModels.ResultModels;

public class GratuityResultViewModel
{
    public required GratuityInputViewModel Inputs { get; init; }

    public long TotalSalaryDrawn { get; init; }

    public required long GratuityAmount { get; init; }

    public string MonthlySalaryStr => Inputs.Salary.ToString("C0", Inputs.Culture);

    public string TotalSalaryDrawnStr => TotalSalaryDrawn.ToString("C0", Inputs.Culture);

    public string GratuityAmountStr => GratuityAmount.ToString("C0", Inputs.Culture);
}