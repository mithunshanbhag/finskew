namespace FinSkew.Ui.Models.ViewModels.ResultModels;

public class XirrResultViewModel
{
    public required XirrInputViewModel Inputs { get; init; }

    public required double Xirr { get; init; }

    public string XirrStr => string.Format(Inputs.Culture, "{0:N2}%", Xirr * 100);
}