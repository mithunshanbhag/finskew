# Step-Up SIP (Systematic Investment Plan) Calculator

## Inputs

The following inputs will be taken from the user:

- M: Initial monthly investment amount
  - Display label: "Monthly Investment Amount"
  - Type: Integer
  - Default value: 1000
  - Minimum value: 500
  - Maximum value: 10000000
  - Step value: 500
- S: Annual step-up percentage applied to the monthly SIP
  - Display label: "Annual Step-Up Percentage"
  - Type: Float or Decimal
  - Default value: 5.0
  - Minimum value: 0
  - Maximum value: 50
  - Step value: 0.5
- R: Expected annual return rate in percentage
  - Display label: "Expected Annual Return Rate"
  - Type: Float or Decimal
  - Default value: 12.0
  - Minimum value: 1
  - Maximum value: 100
  - Step value: 0.5
- N: Time period in years
  - Display label: "Time Period (Years)"
  - Type: Integer
  - Default value: 5
  - Minimum value: 1
  - Maximum value: 50
  - Step value: 1

>Note: Investment frequency is fixed to monthly, and the step-up is applied once per year at the start of each year.

## Outputs

The following output will be shown to the user:

## Chart

- Donut chart
  - "Invested Amount" vs "Total Gain"

## Summary Panel

- P: Total invested amount
  - Display label: "Invested Amount"
- I: Total gain (interest earned)
  - Display label: "Total Gain"
- A: Final amount (after interest is applied)
  - Display label: "Final Amount"

## Calculations

1. Calculate the monthly investment for each year considering the step-up:
   - For year 1: M
   - For year 2: M * (1 + S/100)
   - For year 3: M * (1 + S/100)^2
   - ...
   - For year N: M * (1 + S/100)^(N-1)

2. Calculate the total invested amount (P) by summing the monthly investments for each year:

   ```text
    P = M * 12 * [1 + (1 + S/100) + (1 + S/100)^2 + ... + (1 + S/100)^(N-1)]
   ```

3. Calculate the monthly rate of return (r):

   ```text
    r = R / (12 × 100)
   ```

4. Calculate the final amount (A) by applying the formula for each year's investment and summing them up:

   ```text
    A = M * [((1 + r)^(12 * 1) - 1) / r] * (1 + r) + 
    M * (1 + S/100) * [((1 + r)^(12 * 2) - 1) / r] * (1 + r) + 
    M * (1 + S/100)^2 * [((1 + r)^(12 * 3) - 1) / r] * (1 + r) + 
    ... + 
    M * (1 + S/100)^(N-1) * [((1 + r)^(12 * N) - 1) / r] * (1 + r)
   ```

5. Calculate the total gains (I):

   ```text
    I = A - P
   ```
