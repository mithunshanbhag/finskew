namespace FinSkew.Ui.Services.Implementations;

public abstract class CalculatorBase<TInput, TResult>(IValidator<TInput> validator) : ICalculator<TInput, TResult>
{
    public abstract TResult Compute(TInput input);

    protected void ValidateInput(TInput input)
    {
        validator.ValidateAndThrow(input);
    }
}