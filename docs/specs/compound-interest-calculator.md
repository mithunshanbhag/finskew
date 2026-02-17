# Compound Interest Calculator

## Inputs

The following inputs will be taken from the user:

- P: Principal amount
  - Type: Integer
  - Default value: 10000
  - Minimum value: 10000
  - Maximum value: 100000000
  - Step value: 1000
- R: Annual interest rate in percentage
  - Type: Float or Decimal
  - Default value: 5.0
  - Minimum value: 1
  - Maximum value: 100
  - Step value: 0.5
- N: Time period in years
  - Type: Integer
  - Default value: 3
  - Minimum value: 1
  - Maximum value: 100
  - Step value: 1
- T: Number of times interest is compounded per year (i.e. compounding frequency)
  - Type: Integer (selection)
  - Default value: 4
  - Minimum value: 1
  - Maximum value: 365
  - Step value: 1

## Outputs

The following output will be shown to the user:

- P: Original principal amount
- I: Interest earned
- A: Total amount after interest is applied

## Calculations

The total amount (A) is calculated using the formula:

```text
A = P × (1 + R/(100 × T))^(N × T)
```

The interest earned (I) is calculated as:

```text
I = A - P
```
