# EMI (Equated Monthly Installment) Calculator

## Inputs

The following inputs will be taken from the user:

- P: Principal loan amount
  - Type: Integer
  - Default value: 100000
  - Minimum value: 10000
  - Maximum value: 100000000
  - Step value: 1000
- R: Annual interest rate in percentage
  - Type: Float or Decimal
  - Default value: 8.5
  - Minimum value: 0
  - Maximum value: 100
  - Step value: 0.1
- N: Time period in years (loan tenure)
  - Type: Integer
  - Default value: 20
  - Minimum value: 1
  - Maximum value: 50
  - Step value: 1

## Outputs

The following output will be shown to the user:

- E: Equated Monthly Installment amount
- TP: Total amount paid over the loan tenure (E × number of months)
- TI: Total interest paid over the loan tenure (TP - P)

## Calculations

The monthly interest rate (r) is calculated as:

```text
r = R / (12 × 100)
```

The total number of monthly installments (n) is calculated as:

```text
n = N × 12
```

The EMI is calculated using the formula:

```text
E = P × r × (1 + r)^n / ((1 + r)^n - 1)
```

The total payment over the loan tenure is calculated as:

```text
TP = E × n
```

The total interest paid over the loan tenure is calculated as:

```text
TI = TP - P
```
