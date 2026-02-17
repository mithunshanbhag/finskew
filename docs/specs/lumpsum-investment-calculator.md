# Lumpsum Investment Calculator

## Inputs

The following inputs will be taken from the user:

- P: Principal amount (lumpsum investment)
  - Type: Integer
  - Default value: 10000
  - Minimum value: 10000
  - Maximum value: 100000000
  - Step value: 1000
- R: Expected annual return rate in percentage
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

>Note: Compounding frequency is fixed to annual (once per year, T=1) and is not user-configurable.

## Outputs

The following output will be shown to the user:

- P: Original principal amount (lumpsum investment)
- I: Interest earned
- A: Total amount after interest is applied (maturity amount)

## Calculations

The total amount (A) is calculated using the formula:

```text
A = P × (1 + R/100)^N
```

The interest earned (I) is calculated as:

```text
I = A - P
```
