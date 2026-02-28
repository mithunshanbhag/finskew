# FinSkew “Chaos Monkey” Test Findings (Feb 26 2026)

## Overview
I used the **FinSkew** financial calculator app available at the Azure Static Web Apps URL and its GitHub repository to perform explorative “chaos monkey” tests.  The goal was to identify problems not already described in the repository’s `future.md` file.  The tests covered every calculator (SIP, Lumpsum, Step‑Up SIP, STP, SCSS, EMI, Simple Interest, Compound Interest, CAGR, XIRR, SWP and Gratuity) and concentrated on input validation, state management and UI/UX.  Below are the significant issues discovered.

## Common problems across calculators

### 1. **Decimals and non‑integer inputs are rejected or handled inconsistently**
- Many numeric fields (e.g., invested amount, monthly contribution, time period) accept only whole numbers.  Supplying a **decimal** value silently resets the field to the minimum allowed value (often ₹ 10,000 or ₹ 500) and triggers a persistent red error message “Not a valid number.”  This occurs on **SIP, Lumpsum, Step‑Up SIP, EMI, Simple/Compound Interest, CAGR, XIRR, SWP and Gratuity** calculators.  For example, entering `12345.67` into the SWP *Invested Amount* resets it to `₹10,000` and the error remains【643697495950315†screenshot】; the same behaviour occurs for **Gratuity** when a decimal salary is entered【150843471602936†screenshot】.  Time‑period fields across calculators reject decimals (e.g., `2.5 years`) and revert to the minimum allowed year with an error【355639926703693†screenshot】.  Users therefore cannot model fractional years or partial months, which is a legitimate use case.

### 2. **Out‑of‑range values produce confusing behaviour**
- **Lower than minimum values** (e.g., negative numbers or values smaller than the minimum allowed) reset to the minimum but display “Not a valid number.”  For instance, entering a monthly withdrawal below ₹ 500 in SWP resets it to ₹ 500 and shows the error【245612711749903†screenshot】.  Entering fewer than five years of service in the Gratuity calculator also resets the field to 5 with an error【248840045850716†screenshot】.
- **Excessively large inputs** are sometimes clamped to the maximum value (e.g., large withdrawal amounts are clamped to ₹ 1,00,00,000 in SWP【544609754594770†screenshot】), but sometimes revert to the minimum with an error (e.g., extremely large lumpsum or invested amounts on some calculators).  The lack of consistency confuses users and may lead to unexpected results.

### 3. **Validation errors persist and cannot be cleared easily**
- Each calculator header contains a circular **refresh icon** intended to reset inputs.  However, clicking this icon does **not** remove validation error messages or restore default values.  For example, after entering an invalid time period in SWP, clicking the refresh icon leaves the “Not a valid number” error on screen【422895103192362†screenshot】; the same happens in the Gratuity calculator【201300936376554†screenshot】.  Users must manually enter a valid integer to clear the error.

### 4. **Empty or incomplete calculators**
- The **Systematic Transfer Plan (STP) calculator** page remains blank and is not implemented; it displays a blank page without form inputs or results.
- The **Senior Citizens Savings Scheme (SCSS) calculator** page allows editing only the invested amount and exhibits the same decimal‑input bug as other calculators.  Interest rate and time period are fixed values with no explanation and there is no results table.

## Calculator‑specific findings

### SIP, Lumpsum and Step‑Up SIP
- Decimal amounts or time periods revert to the minimum value and show an error.  In SIP and Step‑Up SIP, entering a decimal monthly investment shows `₹50` with an error (the minimum investment)【84114315988113†screenshot】; in Lumpsum the same occurs for invested amount【543293884651858†screenshot】.
- Excessively large values sometimes cause the field to reset to the minimum rather than the maximum (e.g., entering ₹1 billion in Lumpsum resets to ₹50 rather than clamping to the maximum)【70957980560624†screenshot】.

### Simple & Compound Interest
- Decimal invested amounts are rejected and reset to ₹10,000 with an error【692237091817021†screenshot】【982631003393377†screenshot】.
- Decimal time periods revert to one year and show an error【454871816382158†screenshot】【808352134067589†screenshot】.
- The compounding frequency drop‑down works correctly, but the refresh button still fails to clear errors【766638149551765†screenshot】.

### CAGR
- Decimal values in both the *Invested Amount* and *Final Amount* fields reset to ₹10,000 with an error【805022953657634†screenshot】.
- If the invested amount is higher than the final amount, the calculated CAGR becomes negative; while mathematically correct, negative CAGR should perhaps be displayed differently (e.g., with a minus sign or warning) to avoid confusion【45918274102026†screenshot】.

### XIRR
- The **Monthly Investment Amount** rejects decimals and resets to the minimum ₹500 with an error【131290701275566†screenshot】.
- The start and end dates are chosen via date pickers, and there is no way to input dates manually.  Testing invalid date orders (start date after end date) is difficult.  There is no validation error if the same month is selected for start and end; a single month simulation runs but may not reflect real XIRR calculation behaviour.

### EMI
- Decimal loan amounts revert to ₹10,000 with an error【86761892066348†screenshot】.  Decimal tenure (years) also resets to 1 and shows an error【867650328053752†screenshot】.  Interest rate accepts decimals and clamps between 0–100%.  A table showing yearly breakdown of principal and interest updates correctly, but negative or unrealistic values can produce confusing output.

### SWP (Systematic Withdrawal Plan)
- **Decimal invested amount** is rejected; entering `12345.67` resets it to ₹10,000 with a “Not a valid number” message【643697495950315†screenshot】.
- **Decimal withdrawal** values are not accepted and revert to ₹500 (minimum) with an error【245612711749903†screenshot】.  Withdrawal values larger than the maximum (₹1,00,00,000) are clamped to that maximum【544609754594770†screenshot】.
- **Time period** must be a whole number of years; decimals revert to 1 year and display an error【355639926703693†screenshot】.
- **Refresh icon** does not clear invalid states【422895103192362†screenshot】.
- When withdrawals exceed returns, the final amount can become negative.  The chart and results display negative values (e.g., `-₹7,24,769`), which may confuse users because a real SWP would stop at zero.  Consider limiting the result to zero and issuing a warning.

### Gratuity
- Monthly salary only accepts integers; entering a decimal resets it to ₹10,000 with an error【150843471602936†screenshot】.
- **Years of service** must be an integer between 5 and 50; decimal or lower values revert to 5 and trigger a “Not a valid number” message【248840045850716†screenshot】.  The maximum years clamp to 50.
- The refresh icon does not clear validation errors【201300936376554†screenshot】.

## Usability recommendations

1. **Allow decimal inputs or provide clear feedback:**  Many financial scenarios involve fractional amounts (e.g., ₹12,345.67 invested) and partial years (2.5 years).  Either support decimals or clearly explain the accepted format instead of silently resetting to the minimum value.
2. **Improve range validation:**  Instead of resetting to the minimum, use input constraints (e.g., HTML `min`/`max` attributes) and display a user‑friendly error message specifying the valid range.  Clamping out‑of‑range values should favour the nearest valid bound, not always the minimum.
3. **Fix the refresh/reset button:**  The refresh icon should reset all inputs to their defaults and clear any error messages.  Consider adding a “Reset” button that explicitly performs this action.
4. **Implement missing calculators:**  The STP calculator is currently blank.  The SCSS calculator lacks adjustable interest rate and tenure fields and shows the same validation issues as others; both require implementation.
5. **Prevent negative balances:**  For SWP and other calculators that compute balances over time, guard against scenarios that produce negative final amounts and inform the user when withdrawals exceed the balance.
6. **Enhance date input for XIRR:**  Provide manual date entry or more flexible date pickers, and validate that the end date is after the start date.
7. **Accessibility & UX:**  Apart from the issues noted in the existing `future.md` (e.g., missing skip links, headings, SEO tags, responsive design), adding tooltips or inline error explanations would improve usability.

By addressing these issues, the FinSkew calculators will handle a wider range of real‑world scenarios, provide clearer feedback and avoid confusing results.
