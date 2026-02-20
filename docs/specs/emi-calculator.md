# EMI (Equated Monthly Installment) Calculator

## Inputs

The following inputs will be taken from the user:

- P: Loan amount
  - Display label: "Loan Amount"
  - Type: Integer
  - Default value: 100000
  - Minimum value: 10000
  - Maximum value: 100000000
  - Step value: 1000
- R: Annual interest rate in percentage
  - Display label: "Annual Interest Rate"
  - Type: Float or Decimal
  - Default value: 8.5
  - Minimum value: 0
  - Maximum value: 100
  - Step value: 0.1
- N: Time period in years (loan tenure)
  - Display label: "Loan Tenure (Years)"
  - Type: Integer
  - Default value: 20
  - Minimum value: 1
  - Maximum value: 50
  - Step value: 1

## Outputs

The following output will be shown to the user:

### Chart

- Donut chart
  - "Loan Amount" vs "Total Interest"

### Summary Panel

- P: Total loan amount (echoed back for clarity)
  - Display label: "Loan Amount"
- I: Total interest paid over the loan tenure (A - P)
  - Display label: "Total Interest"
- A: Total amount paid over the loan tenure (EMI × total number of installments)
  - Display label: "Total Amount"
- E: Equated Monthly Installment amount
  - Display label: "Monthly EMI"

### Growth

- Yearly growth table
  - Column 1: Year
  - Column 2: Amount paid towards loan (cumulative)

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
A = E × n
```

The total interest paid over the loan tenure is calculated as:

```text
I = A - P
```

The cumulative amount paid towards the loan at the end of year `Y` is:

```text
Paid(Y) = E × (12 × Y)
```
