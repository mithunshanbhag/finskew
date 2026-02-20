# SWP (Systematic Withdrawal Plan) Calculator

## Inputs

The following inputs will be taken from the user:

- P: Invested amount
  - Display label: "Invested Amount"
  - Type: Integer
  - Default value: 500000
  - Minimum value: 10000
  - Maximum value: 100000000
  - Step value: 1000
- W: Monthly withdrawal amount
  - Display label: "Monthly Withdrawal Amount"
  - Type: Integer
  - Default value: 10000
  - Minimum value: 500
  - Maximum value: 10000000
  - Step value: 500
- R: Expected annual return rate in percentage
  - Display label: "Expected Annual Return Rate"
  - Type: Float or Decimal
  - Default value: 8.0
  - Minimum value: 0
  - Maximum value: 100
  - Step value: 0.5
- N: Time period in years
  - Display label: "Time Period (Years)"
  - Type: Integer
  - Default value: 5
  - Minimum value: 1
  - Maximum value: 50
  - Step value: 1

>Note: Withdrawal frequency is fixed to monthly. Withdrawal happens at the beginning of each month.

## Outputs

The following output will be shown to the user:

### Chart

- Donut chart
  - "Invested Amount" vs "Total Withdrawal"

### Summary Panel

- P: Invested amount (initial investment)
  - Display label: "Invested Amount"
- X: Total amount withdrawn over the withdrawal period
  - Display label: "Total Withdrawal"
- A: Final amount after the withdrawal period
  - Display label: "Final Amount"

### Growth

- Yearly growth table
  - Column 1: Year
  - Column 2: Total Investment (end-of-year corpus; can be negative)

## Calculations

1. Calculate the monthly rate of return (r):

   ```text
    r = R / (12 × 100)
   ```

2. Calculate the total amount withdrawn (X) over the withdrawal period:

   ```text
    X = W × 12 × N
   ```

3. Calculate the final amount (A) after the withdrawal period using the formula:

   ```text
    A = P × (1 + r)^(12 × N) - W × [((1 + r)^(12 × N) - 1) / r] × (1 + r)
   ```

4. Calculate the yearly end-of-year investment value for year `Y` (`1 <= Y <= N`) using the same formula with `12 × Y` months.
