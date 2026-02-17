# CAGR (Compound Annual Growth Rate) Calculator

## Inputs

The following inputs will be taken from the user:

- P: Initial principal amount
  - Type: Integer
  - Default value: 10000
  - Minimum value: 10000
  - Maximum value: 100000000
  - Step value: 1000
- A: Final amount after growth
  - Type: Integer
  - Default value: 12000
  - Minimum value: 10000
  - Maximum value: 100000000
  - Step value: 1000
- N: Time period in years
  - Type: Integer
  - Default value: 3
  - Minimum value: 1
  - Maximum value: 100
  - Step value: 1

## Outputs

The following output will be shown to the user:

- R: Compound Annual Growth Rate (CAGR) in percentage

>Note: No chart will be shown for this calculator since it is a straightforward calculation with a single output.

## Calculations

The CAGR (R) is calculated using the formula:

```text
R = [(A / P)^(1 / N) - 1] × 100
```
