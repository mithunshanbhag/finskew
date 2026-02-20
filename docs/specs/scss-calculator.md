# SCSS (Senior Citizen Savings Scheme) calculator

## Inputs

- P: Invested amount
  - Display label: "Invested Amount"
  - Type: Integer
  - Default value: 10000
  - Minimum value: 10000
  - Maximum value: 100000000
  - Step value: 1000
- R: Annual interest rate in percentage (fixed at 7.4% as per current SCSS rates, but may be updated in the future)
  - Display label: "Annual Interest Rate"
  - Type: Float or Decimal
  - Default value: 7.4
  - Minimum value: N/A
  - Maximum value: N/A
  - Step value: N/A
  - Read only field: true
- N: Time period in years (fixed at 5 years as per SCSS rules, but may be updated in the future)
  - Display label: "Time Period (Years)"
  - Type: Integer
  - Default value: 5
  - Minimum value: N/A
  - Maximum value: N/A
  - Step value: N/A
  - Read only field: true

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

### Growth

- Simple yearly growth chart (bar or line)
  - Year on X-axis and total amount on Y-axis.
  - Single series: end-of-year total value (principal + gains).
- Yearly growth table
  - Column 1: Year
  - Column 2: Growth of Invested Amount (end-of-year total value)

## Calculations

1. Calculate the final amount (A) after 5 years using the formula:

   ```text
    A = P × (1 + R/100)^N
   ```

2. Calculate the total interest earned (I):

   ```text
    I = A - P
   ```
