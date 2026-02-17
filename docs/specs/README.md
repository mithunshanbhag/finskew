# FinSkew: Financial Calculators

This document describes the specifications for the FinSkew: a web application that provides various financial calculators. FinSkew is meant to be an educational tool for individuals looking to understand and manage their finances better.

## Assumptions

In this version, the app will primarily cater to Indian users. This means that:

- The currency will be in Indian Rupees (INR)
- The default interest rates will be based on the Indian financial market.
- The Indian numbering system (where commas are placed after every two digits starting from the right, except for the first three digits) will be used.

> However, in future versions the app will be adapted for international users, by allowing for currency, interest rate adjustments, etc.

## List of calculators

- Income calculators
  - [Gratuity calculator](./gratuity-calculator.md)
- Interest calculators
  - [Simple interest calculator](./simple-interest-calculator.md)
  - [Compound interest calculator](./compound-interest-calculator.md)
  - [CAGR calculator](./cagr-calculator.md)
  - [XIRR calculator](./xirr-calculator.md)
- Investment calculators
  - [Lump sum investment calculator](./lumpsum-investment-calculator.md)
  - [Systematic Investment Plan (SIP) calculator](./sip-calculator.md)
  - [Step-Up SIP calculator](./step-up-sip-calculator.md)
  - [Systematic Transfer Plan (STP) calculator](./stp-calculator.md)
  - [Senior Citizen Savings Scheme (SCSS) calculator](./scss-calculator.md)
- Loan calculators
  - [EMI calculator](./emi-calculator.md)
- Retirement calculators
  - [Systematic Withdrawal Plan (SWP) calculator](./swp-calculator.md)

> Note: The fine-grained requirements about specific calculators (e.g. input fields, calculations, outputs) will be documented in separate markdown files (as linked above) to keep things organized and modular.

## UI specifications

- [UI elements and layout](./ui.md)
