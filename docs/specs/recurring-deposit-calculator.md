# Recurring Deposit Calculator

## Inputs

The following inputs will be taken from the user:

- M: Monthly deposit amount
  - Display label: "Monthly Deposit Amount"
  - Type: Integer
  - Default value: 5000
  - Minimum value: 1000
  - Maximum value: 1000000
  - Step value: 1000
- R: Expected annual interest rate in percentage
  - Display label: "Expected Annual Interest Rate"
  - Type: Float or Decimal
  - Default value: 6.0
  - Minimum value: 0
  - Maximum value: 100
  - Step value: 0.1
- N: Time period in years
  - Display label: "Time Period (Years)"
  - Type: Integer
  - Default value: 5
  - Minimum value: 1
  - Maximum value: 50
  - Step value: 1

>Note: Deposit frequency is fixed to monthly. Deposits are made at the end of each month.

## Outputs

The following output will be shown to the user:

### Chart

- Donut chart
  - "Invested Amount" vs "Total Gain"

### Summary Panel

- P: Total invested amount
  - Display label: "Invested Amount"
- I: Total gain (interest earned)
  - Display label: "Total Gain"
- A: Final amount (after interest is applied)
  - Display label: "Final Amount"

### Growth

- Simple yearly growth chart (bar or line)
  - Year on X-axis and total amount on Y-axis.
  - Single series: end-of-year total value (principal + gains).
- Yearly growth table
  - Column 1: Year
  - Column 2: Growth of Invested Amount (end-of-year total value)

## Calculations

The total invested amount (P) is calculated as:

```text
P = M × 12 × N
```

The final amount (A) is calculated using the formula:

```text
A = M × [((1 + (R/100)/12)^(N*12) - 1) / ((R/100)/12)]
```

The total gain (I) is calculated as:

```text
I = A - P
```
