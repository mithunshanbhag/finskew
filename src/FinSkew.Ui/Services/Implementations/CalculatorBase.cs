namespace FinSkew.Ui.Services.Implementations;

public abstract class CalculatorBase<TInput, TResult> : ICalculator<TInput, TResult>
{
    public abstract TResult Compute(TInput input);
}