# Step-Up SIP (Systematic Investment Plan) Calculator

## Inputs

The following inputs will be taken from the user:

- M: Initial monthly investment amount
- S: Annual step-up percentage applied to the monthly SIP
- R: Expected annual return rate in percentage
- N: Time period in years

>Note: Investment frequency is fixed to monthly, and the step-up is applied once per year at the start of each year.

## Outputs

The following output will be shown to the user:

- P: Total invested amount
- I: Total gain (interest earned)
- A: Total maturity amount after interest is applied

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

4. Calculate the total maturity amount (A) by applying the formula for each year's investment and summing them up:

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
