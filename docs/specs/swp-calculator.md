# SWP (Systematic Withdrawal Plan) Calculator

## Inputs

The following inputs will be taken from the user:

- P: Total investment amount
- W: Monthly withdrawal amount
- R: Expected annual return rate in percentage
- N: Time period in years

>Note: Withdrawal frequency is fixed to monthly. Withdrawal happens at the beginning of each month.

## Outputs

The following output will be shown to the user:

- P: Total invested amount (initial investment)
- X: Total amount withdrawn over the withdrawal period
- A: Total maturity amount after the withdrawal period

## Calculations

1. Calculate the monthly rate of return (r):

   ```text
    r = R / (12 × 100)
   ```

2. Calculate the total amount withdrawn (X) over the withdrawal period:

   ```text
    X = W × 12 × N
   ```

3. Calculate the total maturity amount (A) after the withdrawal period using the formula:

   ```text
    A = P × (1 + r)^(12 × N) - W × [((1 + r)^(12 × N) - 1) / r] × (1 + r)
   ```
