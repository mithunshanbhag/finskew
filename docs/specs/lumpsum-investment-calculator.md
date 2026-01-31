# Lumpsum Investment Calculator

## Inputs

The following inputs will be taken from the user:

- P: Principal amount (lumpsum investment)
- R: Expected annual return rate in percentage
- N: Time period in years

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
