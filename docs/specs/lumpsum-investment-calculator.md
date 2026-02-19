# Lumpsum Investment Calculator

## Inputs

The following inputs will be taken from the user:

- P: Invested amount (lumpsum)
  - Display label: "Invested Amount (Lumpsum)"
  - Type: Integer
  - Default value: 10000
  - Minimum value: 10000
  - Maximum value: 100000000
  - Step value: 1000
- R: Expected annual return rate in percentage
  - Display label: "Expected Annual Return Rate"
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

>Note: Compounding frequency is fixed to annual (once per year, T=1) and is not user-configurable.

## Outputs

The following output will be shown to the user:

## Chart

- Donut chart
  - "Invested Amount" vs "Total Gain"

## Summary Panel

- P: Invested amount (lumpsum) (echoed back for clarity)
  - Display label: "Invested Amount"
- I: Total gain
  - Display label: "Total Gain"
- A: Final amount after interest is applied
  - Display label: "Final Amount"

## Calculations

The total amount (A) is calculated using the formula:

```text
A = P × (1 + R/100)^N
```

The interest earned (I) is calculated as:

```text
I = A - P
```
