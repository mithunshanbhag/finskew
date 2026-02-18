# Compound Interest Calculator

## Inputs

The following inputs will be taken from the user:

- P: Principal amount
  - Display label: "Principal Amount"
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
- T: Number of times interest is compounded per year (i.e. compounding frequency)
  - Display label: "Compounding Frequency"
  - Type: Integer (selection)
  - Default value: 4
  - Minimum value: 1
  - Maximum value: 365
  - Step value: 1

## Outputs

The following output will be shown to the user:

### Chart

- Donut chart
  - "Principal Amount" vs "Interest Earned"

### Summary Panel

- P: Original principal amount
  - Display label: "Principal Amount"
- I: Interest earned
  - Display label: "Interest Earned"
- A: Total amount after interest is applied
  - Display label: "Maturity Amount"

## Calculations

The total amount (A) is calculated using the formula:

```text
A = P × (1 + R/(100 × T))^(N × T)
```

The interest earned (I) is calculated as:

```text
I = A - P
```
