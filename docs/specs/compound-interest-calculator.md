# Compound Interest Calculator

## Inputs

The following inputs will be taken from the user:

- P: Principal amount
- R: Annual interest rate in percentage
- N: Time period in years
- T: Number of times interest is compounded per year (i.e. compounding frequency)

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
