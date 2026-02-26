# Post Office MIS (Monthly Income Scheme) Calculator

## Inputs

The following inputs will be taken from the user:

- P: Invested amount
  - Display label: "Invested Amount"
  - Type: Integer
  - Default value: 100000
  - Minimum value: 10000
  - Maximum value: 100000000
  - Step value: 1000
- R: Annual interest rate in percentage
  - Display label: "Annual Interest Rate"
  - Type: Float or Decimal
  - Default value: 6.6
  - Minimum value: 1
  - Maximum value: 100
  - Step value: 0.1
- N: Time period in years
  - Display label: "Time Period (Years)"
  - Type: Integer
  - Default value: 5
  - Minimum value: 1
  - Maximum value: 50
  - Step value: 1
  - Read only field: true

> Note: The maximum tenure for Post Office MIS is 5 years
>
> Note: The interest is compounded quarterly for Post Office MIS, so the final amount will be higher than simple interest calculation.

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

The total amount (A) is calculated using the formula for compound interest with quarterly compounding:

```text
A = P × (1 + R/100/4)^(4 × N)
```
