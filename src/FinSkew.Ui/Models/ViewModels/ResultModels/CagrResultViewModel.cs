namespace FinSkew.Ui.Models.ViewModels.ResultModels;

public class CagrResultViewModel
{
    public required CagrInputViewModel Inputs { get; init; }

    public required double CompoundAnnualGrowthRate { get; init; }

    public string CompoundAnnualGrowthRateStr => string.Format(Inputs.Culture, "{0:N2}%", CompoundAnnualGrowthRate);
}