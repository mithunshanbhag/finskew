# Gratuity Calculator

## Inputs

The following inputs will be taken from the user:

- S: Salary (basic + dearness allowance)
  - Type: Integer
  - Default value: 50000
  - Minimum value: 10000
  - Maximum value: 100000000
  - Step value: 1000
- Y: Number of years of service
  - Type: Integer
  - Default value: 5
  - Minimum value: 5
  - Maximum value: 50
  - Step value: 1
  - Note: Eligibility requires at least 5 years of service.

## Outputs

The following output will be shown to the user:

- G: Gratuity amount payable

## Calculations

The gratuity amount (G) is calculated using the formula:

```text
G = (S × Y × 15) / 26
```

>Notes:
>
>- 15: Gratuity is calculated as 15 days' wages for every completed year of service
>- 26: Number of working days in a month (used to convert 15 days' wages into a monthly salary equivalent)
