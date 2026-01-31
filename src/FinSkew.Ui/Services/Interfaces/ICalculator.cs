namespace FinSkew.Ui.Services.Interfaces;

public interface ICalculator<in TInput, out TResult>
{
    TResult Compute(TInput input);
}