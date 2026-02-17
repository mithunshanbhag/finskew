# XIRR (Extended Internal Rate of Return) Calculator

## Inputs

The following inputs will be taken from the user:

- S: Investment Start Date
  - Type: Date
  - Default value: Today's date minus 5 years
  - Minimum value: 100 years before today (MudDatePicker `MinDate = DateTime.Today.AddYears(-100)`)
  - Maximum value: One day before the selected Investment Maturity Date (MudDatePicker `MaxDate = InvestmentMaturityDate.AddDays(-1)`)
- E: Investment Maturity Date
  - Type: Date
  - Default value: Today's date
  - Minimum value: One day after the selected Investment Start Date (MudDatePicker `MinDate = InvestmentStartDate.AddDays(1)`)
  - Maximum value: 100 years from today (MudDatePicker `MaxDate = DateTime.Today.AddYears(100)`)
- M: Monthly Investment Amount
  - Type: Integer
  - Default value: 1000
  - Minimum value: 500
  - Maximum value: 10000000
  - Step value: 500
- R: Expected Annual Return Rate
  - Type: Float or Decimal
  - Default value: 12.0
  - Minimum value: 0
  - Maximum value: 100
  - Step value: 0.5

## Outputs

The following output will be shown to the user:

- XIRR: Extended Internal Rate of Return in percentage

## Calculations

The monthly cashflows are constructed from S to E as:

```text
K = number of monthly investment dates where d_i = S + i months and d_i < E
For i = 0 to K - 1:
  C_i = -M at date d_i
```

The projected maturity amount (A) from R is calculated using monthly compounding:

```text
r = R / (12 × 100)
A = M × [((1 + r)^K - 1) / r] × (1 + r)
```

>Note: If R = 0, then A = M × K.

The XIRR is then defined using date-based discounting:

```text
Set t_0 = S, t_K = E, C_K = +A
Find x such that:
Σ(j=0 to K) [ C_j / (1 + x)^((t_j - t_0)/365) ] = 0
```

The XIRR value (x) is solved numerically using Newton-Raphson, with fallback to bracketing/bisection when Newton-Raphson does not converge.
