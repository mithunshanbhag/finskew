# SCSS (Senior Citizen Savings Scheme) calculator

## Inputs

- P: Initial deposit amount
  - Type: Integer
  - Default value: 10000
  - Minimum value: 10000
  - Maximum value: 100000000
  - Step value: 1000

>Notes:
>
>- R: Annual interest rate is fixed at 7.4% (as per current SCSS rates, but may be updated in the future).
>- T: Tenure in years is fixed at 5 years (as per SCSS rules, but may be updated in the future).

## Outputs

- P: Original principal amount (initial deposit)
- I: Total interest earned over the 5-year period
- M: Maturity amount after 5 years

## Calculations

1. Calculate the maturity amount (M) after 5 years using the formula:

   ```text
    M = P × (1 + R/100)^T
   ```

2. Calculate the total interest earned (I):

   ```text
    I = M - P
   ```
