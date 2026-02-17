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

## Future enhancements (not yet implemented)

- Support for **exporting/printing** calculator results (e.g. PDF, CSV) may be added in a future version.
- Input validation & presets: define min/max bounds, step increments, and clear error messages; provide common presets for currency and compounding frequency to reduce user friction.
- Accessibility improvements: add ARIA labels, keyboard navigation, high-contrast mode, screen-reader optimizations, and automated accessibility checks (WCAG AA).
- Detailed breakdown views: show principal vs. gains, year/month-wise amortization tables, and monthly breakdown exports for transparency.
- Comparison & what‑if scenarios: side-by-side comparisons, sensitivity analysis, and a reverse/goal mode that computes required SIP or time to reach a target amount.
- Compounding frequency & time granularity controls: support annual/quarterly/monthly/daily compounding and allow years+months or months-only inputs for finer granularity.
- Inflation & tax-impact toggles: provide optional inflation-adjusted (real) returns and post-tax outcome calculations with configurable tax assumptions.
- Visualizations & interactivity: interactive charts, input sliders, and a real-time formula display with substituted values to educate users on calculations.
- Export & persistence: enable CSV/PDF export, save presets and calculation history, and produce shareable reports including assumptions and disclaimers.
- Localization & currency support: multi-currency formatting, i18n/localization strings, and support for Indian-number formatting as an option.
- Presets, suggestions & historical defaults: offer conservative/moderate/aggressive presets and optional historical rate hints for common asset classes.
- Accessibility & QA in tests: add E2E accessibility audits and unit tests covering rounding, precision, and edge cases (e.g., zero/negative inputs).
- API & integration hooks: design server-side calculation endpoints and optional telemetry/analytics hooks for product insights and heavier computations.
