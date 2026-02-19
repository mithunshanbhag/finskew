# Simple Interest Calculator

## Inputs

The following inputs will be taken from the user:

- P: Invested amount
  - Display label: "Invested Amount"
  - Type: Integer
  - Default value: 10000
  - Minimum value: 10000
  - Maximum value: 100000000
  - Step value: 1000
- R: Annual interest rate in percentage
  - Display label: "Annual Interest Rate"
  - Type: Float or Decimal
  - Default value: 5.0
  - Minimum value: 1
  - Maximum value: 100
  - Step value: 0.5
- N: Time period in years
  - Display label: "Time Period (Years)"
  - Type: Integer
  - Default value: 3
  - Minimum value: 1
  - Maximum value: 100
  - Step value: 1

## Outputs

The following output will be shown to the user:

### Chart

- Donut chart
  - "Invested Amount" vs "Total Gain"

### Summary Panel

- P: Original invested amount
  - Display label: "Invested Amount"
- I: Total gain
  - Display label: "Total Gain"
- A: Final amount (after interest is applied)
  - Display label: "Final Amount"

## Calculations

The total amount (A) is calculated using the formula:

```text
A = P × (1 + (R/100) × N)
```

The interest earned (I) is calculated as:

```text
I = A - P
```
