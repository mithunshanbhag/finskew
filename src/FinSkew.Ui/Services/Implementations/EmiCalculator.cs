namespace FinSkew.Ui.Services.Implementations;

public class EmiCalculator(IValidator<EmiInputViewModel> validator) : CalculatorBase<EmiInputViewModel, EmiResultViewModel>(validator)
{
    public override EmiResultViewModel Compute(EmiInputViewModel input)
    {
        ValidateInput(input);

        var monthlyInterestRate = input.AnnualInterestRate / (12d * 100d);
        var totalInstallments = input.LoanTenureInYears * 12;

        var monthlyEmi = monthlyInterestRate == 0
            ? input.PrincipalAmount / (double)totalInstallments
            : input.PrincipalAmount * monthlyInterestRate * Math.Pow(1 + monthlyInterestRate, totalInstallments)
              / (Math.Pow(1 + monthlyInterestRate, totalInstallments) - 1);

        var totalPayment = (int)(monthlyEmi * totalInstallments);
        var totalInterest = totalPayment - input.PrincipalAmount;
        var yearlyGrowth = new int[input.LoanTenureInYears];
        for (var year = 1; year <= input.LoanTenureInYears; year++) yearlyGrowth[year - 1] = (int)(monthlyEmi * year * 12);

        return new EmiResultViewModel
        {
            Inputs = input,
            MonthlyEmi = (int)monthlyEmi,
            TotalPayment = totalPayment,
            TotalInterest = totalInterest,
            YearlyGrowth = yearlyGrowth
        };
    }
}