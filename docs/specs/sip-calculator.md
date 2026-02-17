# SIP (Systematic Investment Plan) Calculator

## Inputs

The following inputs will be taken from the user:

- M: Monthly investment amount
  - Type: Integer
  - Default value: 1000
  - Minimum value: 500
  - Maximum value: 10000000
  - Step value: 500
- R: Expected annual return rate in percentage
  - Type: Float or Decimal
  - Default value: 12.0
  - Minimum value: 1
  - Maximum value: 100
  - Step value: 0.5
- N: Time period in years
  - Type: Integer
  - Default value: 5
  - Minimum value: 1
  - Maximum value: 50
  - Step value: 1

>Note: Investment frequency is fixed to monthly.

## Outputs

The following output will be shown to the user:

- P: Total invested amount
- I: Total gain (interest earned)
- A: Total maturity amount after interest is applied

## Calculations

The total invested amount (P) is calculated as:

```text
P = M × 12 × N
```

The monthly rate of return (r) is calculated as:

```text
r = R / (12 × 100)
```

The total maturity amount (A) is calculated using the formula:

```text
A = M × [((1 + r)^(12 × N) - 1) / r] × (1 + r)
```

The total gains (I) is calculated as:

```text
I = A - P
```
