# CAGR (Compound Annual Growth Rate) Calculator

## Inputs

The following inputs will be taken from the user:

- P: Initial investment
  - Display label: "Initial Investment"
  - Type: Integer
  - Default value: 10000
  - Minimum value: 10000
  - Maximum value: 100000000
  - Step value: 1000
- A: Final amount
  - Display label: "Final Amount"
  - Type: Integer
  - Default value: 12000
  - Minimum value: 10000
  - Maximum value: 100000000
  - Step value: 1000
- N: Time period in years
  - Display label: "Time Period (Years)"
  - Type: Integer
  - Default value: 3
  - Minimum value: 1
  - Maximum value: 100
  - Step value: 1

## Outputs

The following output will be shown to the user:

### Chart

- Donut chart
  - "Initial Investment" vs "Total Gain"

### Summary Panel
  
- P: Initial investment (echoed back for clarity)
  - Display label: "Initial Investment"
- I: Total gain
  - Display label: "Total Gain"
- A: Final amount (echoed back for clarity)
  - Display label: "Final Amount"
- R: Compound Annual Growth Rate (CAGR) in percentage
  - Display label: "CAGR (%)"

## Calculations

The total gain (I) is calculated as:

```text
I = A - P
```

The CAGR (R) is calculated using the formula:

```text
R = [(A / P)^(1 / N) - 1] × 100
```
