# FinSkew "Chaos Monkey" Test Findings (Feb 26 2026)

## Overview

I used the **FinSkew** financial calculator app available at the Azure Static Web Apps URL and its GitHub repository to perform explorative "chaos monkey" tests. The goal was to identify problems not already described in the repository's `future.md` file. The tests covered every calculator available in the app at the time (SIP, Lumpsum, Step-Up SIP, SCSS, EMI, Simple Interest, Compound Interest, CAGR, XIRR, SWP and Gratuity) and concentrated on input validation, state management and UI/UX. Below are the significant issues discovered, followed by status notes for items that were later improved.

## Common problems across calculators

### 1. **Decimals and non-integer inputs are rejected or handled inconsistently**

- Many numeric fields (e.g., invested amount, monthly contribution, time period) accept only whole numbers. Supplying a **decimal** value silently resets the field to the minimum allowed value (often Rs 10,000 or Rs 500) and triggers a persistent red error message "Not a valid number." This occurs on **SIP, Lumpsum, Step-Up SIP, EMI, Simple/Compound Interest, CAGR, XIRR, SWP and Gratuity** calculators. For example, entering `12345.67` into the SWP *Invested Amount* resets it to `Rs 10,000` and the error remains; the same behaviour occurs for **Gratuity** when a decimal salary is entered. Time-period fields across calculators reject decimals (e.g., `2.5 years`) and revert to the minimum allowed year with an error. Users therefore cannot model fractional years or partial months, which is a legitimate use case.

### 2. **Out-of-range values produce confusing behaviour**

- **Lower than minimum values** (e.g., negative numbers or values smaller than the minimum allowed) reset to the minimum but display "Not a valid number." For instance, entering a monthly withdrawal below Rs 500 in SWP resets it to Rs 500 and shows the error. Entering fewer than five years of service in the Gratuity calculator also resets the field to 5 with an error.
- **Excessively large inputs** are sometimes clamped to the maximum value (e.g., large withdrawal amounts are clamped to Rs 1,00,00,000 in SWP), but sometimes revert to the minimum with an error (e.g., extremely large lumpsum or invested amounts on some calculators). The lack of consistency confuses users and may lead to unexpected results.

### 3. **Validation errors persist and cannot be cleared easily**

- Each calculator header contains a circular **refresh icon** intended to reset inputs. However, clicking this icon does **not** remove validation error messages or restore default values. For example, after entering an invalid time period in SWP, clicking the refresh icon leaves the "Not a valid number" error on screen; the same happens in the Gratuity calculator. Users must manually enter a valid integer to clear the error.

### 4. **Partially implemented calculator UX**

- The **Senior Citizens Savings Scheme (SCSS) calculator** page allows editing only the invested amount and exhibits the same decimal-input bug as other calculators. At the time of the original audit, interest rate and time period were fixed values with little explanation and the page lacked a fuller results presentation.

## Calculator-specific findings

### SIP, Lumpsum and Step-Up SIP

- Decimal amounts or time periods revert to the minimum value and show an error. In SIP and Step-Up SIP, entering a decimal monthly investment shows `Rs 50` with an error (the minimum investment); in Lumpsum the same occurs for invested amount.
- Excessively large values sometimes cause the field to reset to the minimum rather than the maximum (e.g., entering Rs 1 billion in Lumpsum resets to Rs 50 rather than clamping to the maximum).

### Simple & Compound Interest

- Decimal invested amounts are rejected and reset to Rs 10,000 with an error.
- Decimal time periods revert to one year and show an error.
- The compounding frequency drop-down works correctly, but the refresh button still fails to clear errors.

### CAGR

- Decimal values in both the *Invested Amount* and *Final Amount* fields reset to Rs 10,000 with an error.
- If the invested amount is higher than the final amount, the calculated CAGR becomes negative; while mathematically correct, negative CAGR should perhaps be displayed differently (e.g., with a minus sign or warning) to avoid confusion.

### XIRR

- The **Monthly Investment Amount** rejects decimals and resets to the minimum Rs 500 with an error.
- The start and end dates are chosen via date pickers, and there is no way to input dates manually. Later code changes added explicit date-order validation, but the UX is still picker-only and does not support direct text entry for advanced scenarios.

### EMI

- Decimal loan amounts revert to Rs 10,000 with an error. Decimal tenure (years) also resets to 1 and shows an error. Interest rate accepts decimals and clamps between 0-100%. A table showing yearly breakdown of principal and interest updates correctly, but negative or unrealistic values can produce confusing output.

### SWP (Systematic Withdrawal Plan)

- **Decimal invested amount** is rejected; entering `12345.67` resets it to Rs 10,000 with a "Not a valid number" message.
- **Decimal withdrawal** values are not accepted and revert to Rs 500 (minimum) with an error. Withdrawal values larger than the maximum (Rs 1,00,00,000) are clamped to that maximum.
- **Time period** must be a whole number of years; decimals revert to 1 year and display an error.
- **Refresh icon** does not clear invalid states.
- When withdrawals exceed returns, the final amount can become negative. The chart and results display negative values (e.g., `-Rs 7,24,769`), which may confuse users because a real SWP would stop at zero. Consider limiting the result to zero and issuing a warning.

### Gratuity

- Monthly salary only accepts integers; entering a decimal resets it to Rs 10,000 with an error.
- **Years of service** must be an integer between 5 and 50; decimal or lower values revert to 5 and trigger a "Not a valid number" message. The maximum years clamp to 50.
- The refresh icon does not clear validation errors.

## Usability recommendations

1. **Allow decimal inputs or provide clear feedback:** Many financial scenarios involve fractional amounts (e.g., Rs 12,345.67 invested) and partial years (2.5 years). Either support decimals or clearly explain the accepted format instead of silently resetting to the minimum value.
2. **Improve range validation:** Instead of resetting to the minimum, use input constraints (e.g., HTML `min`/`max` attributes) and display a user-friendly error message specifying the valid range. Clamping out-of-range values should favour the nearest valid bound, not always the minimum.
3. **Fix the refresh/reset button:** The refresh icon should reset all inputs to their defaults and clear any error messages. Consider adding a "Reset" button that explicitly performs this action.
4. **Continue improving partially fixed calculators:** SCSS now explains its fixed interest rate and tenure and shows a results panel, but it still needs the remaining invested-amount validation bug fixed and clearer UX around its read-only inputs.
5. **Prevent negative balances:** For SWP and other calculators that compute balances over time, guard against scenarios that produce negative final amounts and inform the user when withdrawals exceed the balance.
6. **Enhance date input for XIRR:** Provide manual date entry or more flexible date pickers in addition to the current date-order validation.
7. **Accessibility & UX:** Apart from the issues noted in the existing `future.md` (e.g., missing skip links, headings, SEO tags, responsive design), adding tooltips or inline error explanations would improve usability.

By addressing these issues, the FinSkew calculators will handle a wider range of real-world scenarios, provide clearer feedback and avoid confusing results.

## Static Web App config scope note (2026-03-04)

For coverage tracking against `src\FinSkew.Ui\wwwroot\staticwebapp.config.json`:

- Addressed there: HTTP response headers and cache policy only (as documented in `docs\specs\future.md` update).
- Not addressable via `staticwebapp.config.json`: the issues in this document are primarily calculator/input UX and behavior concerns (decimal handling, range validation messaging, reset behavior, negative-balance handling, and XIRR date-entry UX), which require Razor/component/service changes.

## Follow-up audit (2026-04-15)

This follow-up was run against the live site at `https://gentle-island-0ba27e300.6.azurestaticapps.net` using the Chrome DevTools MCP server. The deployed app is serving AOT/native Blazor assets (`_framework/dotnet.native.*`), so these findings reflect the AOT + trimmed deployment rather than a local Debug build. Note that the Lighthouse tool available here reports **Accessibility**, **Best Practices**, and **SEO** categories only; it does not provide the Lighthouse **Performance** score, so this update does not quantify the page-weight delta directly.

### Resolved or partially resolved since the original Feb 26 findings

1. **SCSS has improved, but is not fully fixed.** The page now explains that the annual interest rate is fixed at `7.4%` and the tenure is fixed at `5 years`, and it renders a summary results panel. However, the invested-amount decimal-input bug still reproduces.
2. **XIRR now validates date order in code and tests.** The remaining gap is not missing start/end ordering validation; it is the limited, picker-only date-entry UX.

### Still reproducible in live spot checks

- **SIP** still rejects decimal monthly investment values. Entering `12345.67` reset the field to `Rs 500` and displayed the validation message **"Not a valid number"**.
- The **refresh/reset icon** still does not clear invalid state. After forcing the SIP decimal-input error above, clicking the refresh button left both the invalid value state and the error message visible.
- **SCSS** still rejects decimal invested amounts. Entering `12345.67` reset the field to `Rs 10,000` and displayed **"Not a valid number"**.

These live checks suggest that the main calculator-validation and reset-behavior issues documented above should still be treated as open unless they are re-tested calculator-by-calculator.

### Lighthouse follow-up findings

- Lighthouse on both desktop (`/`) and mobile (`/scss-calculator`) reported **Accessibility 97**, **Best Practices 100**, and **SEO 82**.
- **New issue found via Lighthouse:** calculator-page breadcrumb links and helper text are slightly below the required contrast threshold. Lighthouse flagged the breadcrumb link text at **1.87:1** contrast (`#bdbdbd` on white) and several helper-text strings at **4.47:1** contrast (`#747474` on `#fafafa`), which is just under the required **4.5:1** ratio.
- **Still unresolved from `future.md` rather than this file:** the site still lacks a meta description, and `robots.txt` is now **invalid** rather than simply missing because the route appears to return the app shell HTML instead of valid robots directives.

In short: the AOT + trimming deployment is live, but it does **not** resolve the calculator-input and reset issues captured in this document. The meaningful status changes since Feb 26 are the partial SCSS UX improvement, the addition of XIRR date-order validation, and the new Lighthouse-detected color-contrast issue.
