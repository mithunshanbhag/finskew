namespace FinSkew.Ui.Models.ViewModels.InputModels;

public class GratuityInputViewModel
{
    public int Salary { get; set; } = 50000;

    public int YearsOfService { get; set; } = 5;

    public CultureInfo Culture { get; } = CultureInfo.GetCultureInfo("en-In");
}