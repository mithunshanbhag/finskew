namespace FinSkew.Ui.Services.Implementations;

public class EmiCalculator : CalculatorBase<EmiInputViewModel, EmiResultViewModel>
{
    public override EmiResultViewModel Compute(EmiInputViewModel input)
    {
        var monthlyInterestRate = input.AnnualInterestRate / (12d * 100d);
        var totalInstallments = input.LoanTenureInYears * 12;

        if (totalInstallments <= 0)
            return new EmiResultViewModel
            {
                Inputs = input,
                MonthlyEmi = 0,
                TotalPayment = 0,
                TotalInterest = 0
            };

        var monthlyEmi = monthlyInterestRate == 0
            ? input.PrincipalAmount / (double)totalInstallments
            : input.PrincipalAmount * monthlyInterestRate * Math.Pow(1 + monthlyInterestRate, totalInstallments)
              / (Math.Pow(1 + monthlyInterestRate, totalInstallments) - 1);

        var totalPayment = (int)(monthlyEmi * totalInstallments);
        var totalInterest = totalPayment - input.PrincipalAmount;

        return new EmiResultViewModel
        {
            Inputs = input,
            MonthlyEmi = (int)monthlyEmi,
            TotalPayment = totalPayment,
            TotalInterest = totalInterest
        };
    }
}