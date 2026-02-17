# XIRR (Extended Internal Rate of Return) Calculator

## Inputs

The following inputs will be taken from the user:

- S: Investment Start Date
- E: Investment Maturity Date
- M: Monthly Investment Amount
- R: Expected Annual Return Rate

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
