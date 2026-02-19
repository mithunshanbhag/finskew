# SCSS (Senior Citizen Savings Scheme) calculator

## Inputs

- P: Invested amount
  - Display label: "Invested Amount"
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

The following output will be shown to the user:

### Chart

- Donut chart
  - "Invested Amount" vs "Total Gain"

### Summary Panel

- P: Invested amount (echoed back for clarity)
  - Display label: "Invested Amount"
- I: Total gain over the 5-year period
  - Display label: "Total Gain"
- A: Final amount (after 5 years)
  - Display label: "Final Amount"

## Calculations

1. Calculate the final amount (A) after 5 years using the formula:

   ```text
    A = P × (1 + R/100)^T
   ```

2. Calculate the total interest earned (I):

   ```text
    I = A - P
   ```
