# SWP (Systematic Withdrawal Plan) Calculator

## Inputs

The following inputs will be taken from the user:

- P: Total investment amount
  - Type: Integer
  - Default value: 500000
  - Minimum value: 10000
  - Maximum value: 100000000
  - Step value: 1000
- W: Monthly withdrawal amount
  - Type: Integer
  - Default value: 10000
  - Minimum value: 500
  - Maximum value: 10000000
  - Step value: 500
- R: Expected annual return rate in percentage
  - Type: Float or Decimal
  - Default value: 8.0
  - Minimum value: 0
  - Maximum value: 100
  - Step value: 0.5
- N: Time period in years
  - Type: Integer
  - Default value: 5
  - Minimum value: 1
  - Maximum value: 50
  - Step value: 1

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
